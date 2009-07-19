using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using System.IO;
using System.Xml.Linq;
using Microsoft.Web.Administration;
using System.Net;
using Rensoft.ServerManagement.Security;
using System.Security.Principal;
using Rensoft.Hosting.Server.Managers.Config;
using System.Security.AccessControl;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("Website")]
    public class WebsiteManager : RhspManager
    {
        private IscBindManager dnsManager;
        private ServerManager iisManager;
        private WindowsUserManager wuManager;

        public WebsiteManager()
        {
            Load += new EventHandler(WebsiteManager_Load);
        }

        void WebsiteManager_Load(object sender, EventArgs e)
        {
            wuManager = new WindowsUserManager(ServerConfig.WindowsServerName);
            dnsManager = CreateManager<IscBindManager>();
            iisManager = new ServerManager();
        }

        [RhspModuleMethod]
        public Website[] GetFromCustomer(RhspDataID customerID)
        {
            return getQuery<Website>().Where(w => w.CustomerID == customerID).ToArray();
        }

        [RhspModuleMethod]
        public void Create(Website website)
        {
            setDependantProperties(website);
            enforceConstraints(website);

            // Create first, so clean version has correct values.
            Context.HostingConfig.Create(website);

            // Process hosts so primary host is known when getting clean site.
            processWebsiteHosts(website);

            // Get a clean website in case passwords, etc have been changed.
            CleanWebsite cleanWebsite = getClean(website.DataID);

            // Generate a password, and set the initial read only values.
            cleanWebsite.GenerateIisPassword(this);

            // Then syncronize hosting (affects the clean website).
            syncronizeHosting(website, cleanWebsite);

            // Sets the clean values to the dirty website.
            website.TakeCleanValues(cleanWebsite);

            // Update afterwards so new IDs can be saved.
            Context.HostingConfig.Update(website);
        }

        [RhspModuleMethod]
        public void Update(Website website)
        {
            setDependantProperties(website);
            enforceConstraints(website);

            // Call before update, so the previous directory can be discovered.
            moveChangedDirectory(website);

            // Update the config first, so the clean website reflects new values.
            Context.HostingConfig.Update(website);

            // Process hosts so primary host is known when getting clean site.
            processWebsiteHosts(website);

            // Get a clean website in case read only values have been changed.
            CleanWebsite cleanWebsite = getClean(website.DataID);

            // Generate a new password only if empty (i.e. not set yet).
            cleanWebsite.GenerateIisPasswordIfEmpty(this);
            
            // Sync first, as IDs may change.
            syncronizeHosting(website, cleanWebsite);

            // Sets the clean values to the dirty website.
            website.TakeCleanValues(cleanWebsite);

            // Update afterwards so new IDs can be saved.
            Context.HostingConfig.Update(website);
        }

        private void moveChangedDirectory(Website currentWebsite)
        {
            CleanWebsite previousWebsite = getClean(currentWebsite.DataID);
            DirectoryInfo previousDirectory = getWebsiteDirectory(previousWebsite);
            DirectoryInfo currentDirectory = getWebsiteDirectory(currentWebsite);

            if (previousDirectory.FullName != currentDirectory.FullName)
            {
                // If target already exists, make obsolete.
                MakeDirectoryObsolete(currentDirectory);

                // First remove security from previous path to avoid security leaks.
                clearFileZillaSecurity(previousDirectory);

                try
                {
                    // Only move if website requires a directory (and needs it).
                    if (previousDirectory.Exists)
                    {
                        // If the directory has changed, move it.
                        previousDirectory.MoveTo(currentDirectory.FullName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "Could not move website directory '" + previousDirectory + "' to '" +
                        currentDirectory + "'.", ex);
                }
            }
        }

        private DirectoryInfo getWebsiteDirectory(Website website)
        {
            return website.GetDirectory(ServerConfig.WebsiteDirectory);
        }

        private void processWebsiteHost(WebsiteHost h)
        {
            switch (h.PendingAction)
            {
                case ChildPendingAction.Create:
                    HostingConfig.Create(h);
                    break;

                case ChildPendingAction.Update:
                    HostingConfig.Update(h);
                    break;

                case ChildPendingAction.Delete:
                    HostingConfig.Delete<WebsiteHost>(h.DataID);
                    break;
            }
        }

        [RhspModuleMethod]
        public WebsiteDeleteResult Delete(RhspDataID dataID)
        {
            WebsiteDeleteResult r = new WebsiteDeleteResult();

            CleanWebsite website = Get<CleanWebsite>(dataID);
            DeleteHostingResult dhr = deleteHosting(website);
            if (dhr.Errors.Count() != 0)
            {
                r.Errors.AddRange(dhr.Errors.Select(e => e.Message));
            }

            IEnumerable<IRhspDataChild> cd = website.GetDataChildren();
            cd.OfType<DnsZone>().ToList().ForEach(dz => deleteDnsZone(dz));
            cd.OfType<SecurityTemplate>().ToList().ForEach(st => deleteSecurityTemplate(st));
            cd.OfType<WebsiteHost>().ToList().ForEach(wh => deleteWebsiteHost(wh));

            // Delete website after dependancies have been deleted.
            Context.HostingConfig.Delete<Website>(dataID);

            return r;
        }

        public class DeleteHostingResult
        {
            public List<Exception> Errors = new List<Exception>();
        }

        private DeleteHostingResult deleteHosting(CleanWebsite website)
        {
            DeleteHostingResult r = new DeleteHostingResult();

            deleteIisObjects(website);
            clearFileZillaSecurity(getWebsiteDirectory(website));
            tryRemoveIisIdentity(website);
            removeWebsiteSecurity(website);

            MakeDirectoryObsoleteResult mdor = TryMakeDirectoryObsolete(getWebsiteDirectory(website));
            if (mdor.Error != null)
            {
                r.Errors.Add(mdor.Error);
            }

            return r;
        }

        private void removeWebsiteSecurity(CleanWebsite website)
        {
            SecurityIdentifier sid = null;
            if (website.IisSite.IdentitySid != null)
            {
                // In most cases, the SID will exist.
                sid = new SecurityIdentifier(website.IisSite.IdentitySid);
            }
            else if (!string.IsNullOrEmpty(website.IisSite.IdentityUserName))
            {
                // In some cases because of an earlier bug, only the username may exist.
                WindowsUser windowsUser = wuManager.Find(website.IisSite.IdentityUserName);
                if (windowsUser != null)
                {
                    sid = windowsUser.Sid;
                }
            }

            if (sid != null)
            {
                // If no record of the user exists, then we can't remove security.
                removeSecurityRecursive(getWebsiteDirectory(website), sid);
            }
        }

        private void tryRemoveIisIdentity(CleanWebsite website)
        {
            if (wuManager.Exists(website.IisSite.IdentityUserName))
            {
                wuManager.Delete(website.IisSite.IdentityUserName);
            }
        }

        private void clearFileZillaSecurity(DirectoryInfo directory)
        {
            FileZillaManager fzm = CreateManager<FileZillaManager>();
            fzm.ClearGroupPermissions(directory);
            fzm.ClearUserPermissions(directory);
        }

        private void deleteIisObjects(CleanWebsite website)
        {
            var siteQuery = iisManager.Sites.Where(s => s.Id == website.IisSite.SiteID);
            var apQuery = iisManager.ApplicationPools.Where(ap => ap.Name == website.IisSite.ApplicationPoolName);

            if (siteQuery.Count() != 0)
            {
                iisManager.Sites.Remove(siteQuery.Single());
            }

            if (apQuery.Count() != 0)
            {
                iisManager.ApplicationPools.Remove(apQuery.Single());
            }

            iisManager.CommitChanges();
        }

        private void deleteWebsiteHost(WebsiteHost websiteHost)
        {
            if (GetDataSchemaVersion<WebsiteHost>(websiteHost.DataID) >= 2)
            {
                HostingConfig.Delete<WebsiteHost>(websiteHost.DataID);
            }
        }

        private CleanWebsite getClean(RhspDataID dataID)
        {
            return Get<CleanWebsite>(dataID);
        }

        private void setDependantProperties(Website website)
        {
            website.Customer = getCustomer(website.CustomerID);
            website.IisSite.Website = website;
        }

        private Customer getCustomer(RhspDataID customerID)
        {
            return CreateManager<CustomerManager>().Get(customerID);
        }

        private void deleteDnsZone(DnsZone dz)
        {
            CreateManager<DnsZoneManager>().Delete(dz.DataID);
        }

        private void deleteSecurityTemplate(SecurityTemplate st)
        {
            CreateManager<SecurityTemplateManager>().Delete(st.DataID);
        }

        [RhspModuleMethod]
        public Website Get(RhspDataID dataID)
        {
            return Get<Website>(dataID);
        }

        public TWebsite Get<TWebsite>(RhspDataID dataID)
            where TWebsite : Website
        {
            try
            {
                return getQuery<TWebsite>().Where(w => w.DataID == dataID).Single();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get website where ID is '" + dataID + "'.", ex);
            }
        }

        [RhspModuleMethod]
        public Website[] GetAll()
        {
            return getQuery<Website>().ToArray();
        }

        private IEnumerable<TWebsite> getQuery<TWebsite>()
            where TWebsite : Website
        {
            var q = from website in HostingConfig.GetQueryForAllOfType<TWebsite>()
                    join customer in HostingConfig.GetQueryForAllOfType<Customer>()
                        on website.CustomerID equals customer.DataID into customerGroupJoin
                    from joinCustomer in customerGroupJoin.DefaultIfEmpty()
                    join host in HostingConfig.GetQueryForAllOfType<WebsiteHost>()
                        on website.DataID equals host.WebsiteID into hostGroupJoin
                    join dnsZone in HostingConfig.GetQueryForAllOfType<DnsZone>()
                        on website.DataID equals dnsZone.WebsiteID into dnsZoneGroupJoin
                    join security in HostingConfig.GetQueryForAllOfType<SecurityTemplate>()
                        on website.DataID equals security.WebsiteID into securityGroupJoin
                    orderby getVersionBasedPrimaryHostName<TWebsite>(website, hostGroupJoin)
                    select combineWebsite<TWebsite>(
                        website,
                        joinCustomer,
                        hostGroupJoin,
                        dnsZoneGroupJoin,
                        securityGroupJoin);

            return q;
        }

        private string getVersionBasedPrimaryHostName<TWebsite>(
            TWebsite website,
            IEnumerable<WebsiteHost> hostGroupJoin) 
            where TWebsite : Website
        {
            WebsiteHost host;
            if (GetDataSchemaVersion<TWebsite>(website.DataID) >= 2)
            {
                // In version 2, select from the supplied group join (LINQ generated).
                host = hostGroupJoin.Where(h => h.DataID == website.PrimaryHostID).SingleOrDefault();
            }
            else
            {
                // In first version, the host array is pre-populated (select where Primary).
                host = website.HostArray.Where(h => h.Primary).SingleOrDefault();
            }

            if (host != null)
            {
                return host.Name;
            }
            else
            {
                return null;
            }
        }

        private TWebsite combineWebsite<TWebsite>(
            TWebsite website,
            Customer joinCustomer,
            IEnumerable<WebsiteHost> hostGroupJoin, 
            IEnumerable<DnsZone> dnsZoneGroupJoin, 
            IEnumerable<SecurityTemplate> securityGroupJoin)
            where TWebsite : Website
        {
            website.Customer = joinCustomer;
            website.DnsZoneArray = dnsZoneGroupJoin.ToArray();
            website.SecurityArray = securityGroupJoin.ToArray();

            if (GetDataSchemaVersion<TWebsite>(website.DataID) >= 2)
            {
                /* In version 2 and later, set based on the LINQ 
                 * group join. In earlier versions, the hosts are
                 * taken from within the actual website (and so
                 * are set when the website is parsed from XML). */
                website.HostArray = hostGroupJoin.ToArray();
            }
            else
            {
                // Assign a new ID to each version 1 host so that it can be updated by client.
                website.HostArray.ToList().ForEach(h => upgradeVersion1Host(h));

                /* In version 1, the primary host ID did not exist, so calculate what it should be now. */
                website.PrimaryHostID = website.HostArray.Where(h => h.Primary).Single().DataID;
            }

            SetParent(website, website.DnsZoneArray);
            SetParent(website, website.SecurityArray);
            SetParent(website, website.HostArray);

            return website;
        }

        private static void upgradeVersion1Host(WebsiteHost h)
        {
            h.DataID = RhspDataID.Generate();
            h.PendingAction = ChildPendingAction.Create;
        }

        private void enforceConstraints(Website dirtyWebsite)
        {
            if (string.IsNullOrEmpty(dirtyWebsite.PrimaryHost.Name))
            {
                throw new Exception(
                    "Primary host name cannot be null or empty.");
            }

            if (dirtyWebsite.PrimaryHost.Port != WebsiteHost.DefaultHttpPort)
            {
                throw new Exception(
                    "Primary host must use the default http " +
                    "port (" + WebsiteHost.DefaultHttpPort + ").");
            }

            if (dirtyWebsite.PrimaryHost.Protocol != WebsiteHostProtocol.Http)
            {
                throw new Exception(
                    "Primary host must use the http protocol.");
            }

            if (dirtyWebsite.CustomerID == null)
            {
                throw new Exception(
                    "Customer ID was not defined for website.");
            }

            WebsiteHost[] conflictArray;
            if (ExistsWithAnyHost(dirtyWebsite, out conflictArray))
            {
                throw new Exception(
                    "A website already exists with host names: "
                    + string.Join(", ", conflictArray.Select(h => h.Name).ToArray()) + ".");
            }

            if (dirtyWebsite.PrimaryHostID == null)
            {
                throw new Exception(
                    "Primary host ID was not set.");
            }

            if (HostingConfig.Exists<Website>(dirtyWebsite.DataID))
            {
                CleanWebsite cleanWebsite = getClean(dirtyWebsite.DataID);

                if (cleanWebsite.CustomerID != dirtyWebsite.CustomerID)
                {
                    throw new Exception("Customer cannot be changed.");
                }
            }
        }

        private WebsiteHost GetPrimaryHost(Website website)
        {
            if ((website.HostArray == null) || (website.HostArray.Length == 0))
            {
                throw new Exception(
                    "Primary host cannot be found in null or empty array of hosts.");
            }

            WebsiteHost host;
            if (GetDataSchemaVersion(website.DataID, website.GetType()) == RhspData.FirstSchemaVersion)
            {
                // In first version websites, the hosts are stored within the website.
                host = website.HostArray.ToList().Find(h => h.Primary);
            }
            else
            {
                // In version 2 websites, hosts are separate to the website XML hieracrchy.
                host = website.HostArray.ToList().Find(h => h.WebsiteID == website.PrimaryHostID);
            }

            if (host == null)
            {
                throw new Exception(
                    "No primary host was set.");
            }

            return host;
        }

        [RhspModuleMethod]
        public bool SecurityValid(Website website, string currentPrimaryHostName)
        {
            // Ensure there is a customer object so we can get the website directory.
            website.Customer = CreateManager<CustomerManager>().Get(website.CustomerID);

            // Use old directory, as it may be different in the new site (i.e. not yet exists).
            DirectoryInfo websiteDirectory = getDirectory(website.Customer, currentPrimaryHostName);

            // Set the website so relative so that relativePathAndUserExists method can work.
            CleanWebsite cleanWebsite = getClean(website.DataID);
            website.SecurityArray.ToList().ForEach(s => s.Website = cleanWebsite);

            // TODO: check if in create mode, then bypass relative path exists? (only if root)
            var q = from st in website.SecurityArray
                    where

                        // Ignore records that will be deleted.
                        st.PendingAction != ChildPendingAction.Delete
                        
                        // Website is invalid only if relative path doesn't refer to root.
                        && !relativePathIsRoot(st)
                        
                        // Website is invalid if either path or user does not exist.
                        && relativePathAndUserExists(st, websiteDirectory)

                    select st;

            // If 0, then no invalid security found.
            return q.Count() == 0;
        }

        private DirectoryInfo getDirectory(Customer customer, string websiteHostName)
        {
            return Website.GetDirectory(ServerConfig.WebsiteDirectory, customer, websiteHostName);
        }

        private bool relativePathIsRoot(SecurityTemplate st)
        {
            return string.IsNullOrEmpty(st.RelativePath)
                || (st.RelativePath == @"\")
                || (st.RelativePath == "/");
        }

        private bool relativePathAndUserExists(SecurityTemplate st, DirectoryInfo websiteDirectory)
        {
            SecurityTemplateManager stm = CreateManager<SecurityTemplateManager>();
            WindowsUserManager wum = new WindowsUserManager(ServerConfig.WindowsServerName);

            return !stm.RelativePathExists(websiteDirectory, st.RelativePath)
                || (!wum.Exists(st.Username) && !st.UseIisIdentity);
        }

        [RhspModuleMethod]
        public bool ExistsWithAnyHost(Website checkWebsite)
        {
            WebsiteHost[] conflictArray;
            return ExistsWithAnyHost(checkWebsite, out conflictArray);
        }

        [RhspModuleMethod]
        public bool ExistsWithAnyHost(
            Website checkWebsite,
            out WebsiteHost[] conflictArray)
        {
            var q = from host in
                        // Check all sites except this one.
                        from website in GetAll()
                        where website.DataID != checkWebsite.DataID
                        select website.HostArray
                    where host.Any(h1 => checkWebsite.HostArray.Any(
                        h2 => (!WebsiteHost.IsDeleteOrDiscard(h2) && hostsEqual(h1, h2))))
                    select host;

            if (q.Count() != 0)
            {
                // Select the first found host of the first range.
                conflictArray = q.First().ToArray();
                return true;
            }
            else
            {
                conflictArray = new WebsiteHost[0];
                return false;
            }
        }

        private bool hostsEqual(WebsiteHost h1, WebsiteHost h2)
        {
            string h1Name = string.IsNullOrEmpty(h1.Name) ? string.Empty : h1.Name;
            string h2Name = string.IsNullOrEmpty(h2.Name) ? string.Empty : h2.Name;

            return (h1Name == h2Name)
                && (h1.Port == h2.Port)
                && (h1.IpAddress == h2.IpAddress)
                && (h1.Protocol == h2.Protocol);
        }

        private void syncronizeHosting(Website dirtyWebsite, CleanWebsite cleanWebsite)
        {
            // Where the IIS site ID has not been set and the IIS is due to be created, get ID ready.
            if ((cleanWebsite.IisSite.SiteID == 0)
                && (cleanWebsite.IisSite.Mode != WebsiteIisMode.Disabled))
            {
                // Get the next ID and hope it doesn't get used before site is created.
                long nextSiteID = getNextIisSiteID();

                // Ensure that both clean and dirty know about the new ID.
                cleanWebsite.IisSite.SiteID = nextSiteID;
                dirtyWebsite.IisSite.SiteID = nextSiteID;
            }

            // PrimaryHost relies on HostArray which may be empty in the clean version.
            cleanWebsite.HostArray = dirtyWebsite.HostArray;

            syncronizeDirectory(cleanWebsite);
            syncronizeIisIdentity(cleanWebsite);

            // IIS identity sync may have set a new SID.
            dirtyWebsite.IisSite.IdentitySid = cleanWebsite.IisSite.IdentitySid;

            processSecurity(dirtyWebsite);
            syncronizeIisAppPool(cleanWebsite);
            syncronizeIisSite(cleanWebsite);
            processDnsZones(dirtyWebsite);
            syncronizeFtpServer(cleanWebsite);
        }

        private void processWebsiteHosts(Website website)
        {
            // Process any changes in the hosts (XML only, not IIS).{
            website.HostArray.ToList().ForEach(h => processWebsiteHost(h));
        }

        private void syncronizeIisIdentity(CleanWebsite website)
        {
            // Only create or update when IIS is enabled.
            if (website.IisSite.Mode != WebsiteIisMode.Disabled)
            {
                // Create the user if it does not exist and IIS is not disabled.
                if (!wuManager.Exists(website.IisSite.IdentityUserName))
                {
                    SecurityIdentifier sid = wuManager.Create(website.GetIisIdentity(this));
                    website.IisSite.IdentitySid = sid.Value;
                }
                else
                {
                    // Ensure that SID is kept up to date (as it may become out of sync).
                    website.IisSite.IdentitySid = wuManager.Get(website.IisSite.IdentityUserName).Sid.Value;

                    // Update user in case site name has changed.
                    wuManager.Update(website.GetIisIdentity(this));

                    // Ensure that the password is correct.
                    wuManager.SetPassword(
                        new SecurityIdentifier(website.IisSite.IdentitySid),
                        DecryptPassword(website.IisSite.IdentityPassword));
                }
            }
            else
            {
                // If user exists, then remove the user to keep the system tidy.
                tryRemoveIisIdentity(website);

                // Remove directory security belonging to user to keep system tidy.
                removeWebsiteSecurity(website);
            }
        }

        private void removeSecurityRecursive(FileSystemInfo info, SecurityIdentifier sid)
        {
            if (info.Exists)
            {
                SecurityTemplateManager stm = CreateManager<SecurityTemplateManager>();

                if (info is DirectoryInfo)
                {
                    DirectoryInfo di = (DirectoryInfo)info;
                    DirectorySecurity ds = di.GetAccessControl();
                    stm.RemoveMatchingRules(ds, sid, AccessControlType.Allow);
                    stm.RemoveMatchingRules(ds, sid, AccessControlType.Deny);
                    di.SetAccessControl(ds);

                    // Recursively remove security from files and directories.
                    di.GetFiles().ToList().ForEach(p => removeSecurityRecursive(p, sid));
                    di.GetDirectories().ToList().ForEach(p => removeSecurityRecursive(p, sid));
                }
                else if (info is FileInfo)
                {
                    FileInfo fi = (FileInfo)info;
                    FileSecurity fs = fi.GetAccessControl();
                    stm.RemoveMatchingRules(fs, sid, AccessControlType.Allow);
                    stm.RemoveMatchingRules(fs, sid, AccessControlType.Deny);
                    fi.SetAccessControl(fs);
                }
            }
        }

        private void syncronizeIisAppPool(CleanWebsite website)
        {
            var q = from a in iisManager.ApplicationPools
                    where a.Name == website.IisSite.ApplicationPoolName
                    select a;

            ApplicationPool ap = null;
            if (q.Count() != 0)
            {
                ap = q.First();

                if (website.IisSite.Mode != WebsiteIisMode.Disabled)
                {
                    // Update existing app pool settings.
                    applyIisApplicationPoolValues(website, ap);
                }
                else
                {
                    // Delete the application pool when it's not needed.
                    iisManager.ApplicationPools.Remove(ap);
                }
            }
            else
            {
                if (website.IisSite.Mode != WebsiteIisMode.Disabled)
                {
                    // Only create the IIS app pool when site isn't disabled.
                    ap = iisManager.ApplicationPools.Add(website.IisSite.ApplicationPoolName);
                    applyIisApplicationPoolValues(website, ap);
                }
            }

            iisManager.CommitChanges();
        }

        private void applyIisApplicationPoolValues(CleanWebsite website, ApplicationPool ap)
        {
            ap.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
            ap.ProcessModel.UserName = website.IisSite.IdentityUserName;
            ap.ProcessModel.Password = DecryptPassword(website.IisSite.IdentityPassword);
            ap.ManagedRuntimeVersion = website.IisSite.ManagedRuntimeVersion;

            switch (website.IisSite.ManagedPipelineMode)
            {
                case WebsiteIisManagedPipelineMode.Classic:
                    ap.ManagedPipelineMode = ManagedPipelineMode.Classic;
                    break;

                case WebsiteIisManagedPipelineMode.Integrated:
                    ap.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                    break;
            }
        }

        private void syncronizeFtpServer(CleanWebsite website)
        {
            if (website.Customer.FtpEnabled)
            {
                SetDefaultWebsiteSecurity(website);
            }
        }

        public void SetDefaultWebsiteSecurity(Website website)
        {
            CreateManager<FileZillaManager>().SetGroupPermissions(
                website.Customer.FtpGroupName,
                website.GetDirectory(ServerConfig.WebsiteDirectory),
                FtpPermissions.AllowAll);
        }

        private void processDnsZones(Website website)
        {
            foreach (DnsZone dnsZone in website.DnsZoneArray)
            {
                CreateManager<DnsZoneManager>().Process(dnsZone);
            }
        }

        private void processSecurity(Website website)
        {
            foreach (SecurityTemplate securityTemplate in website.SecurityArray)
            {
                securityTemplate.Website = website;
                CreateManager<SecurityTemplateManager>().Process(securityTemplate);
            }
        }

        private void syncronizeIisSite(CleanWebsite website)
        {
            var sq = from s in iisManager.Sites
                     where s.Id == website.IisSite.SiteID
                     select s;

            Site site = null;
            if (sq.Count() != 0)
            {
                site = sq.First();
                if (website.IisSite.Mode != WebsiteIisMode.Disabled)
                {
                    // Simply update the site and it's structure.
                    applySiteValues(website, site);
                    updateIisSite(website, site);

                    // Commit the update before updating redirect (name could have changed).
                    iisManager.CommitChanges();

                    // Update redirect once any name changes have been made in XML config.
                    updateIisRedirect(website, site.GetWebConfiguration());
                }
                else
                {
                    // Remove the site as it shouldn't exist.
                    iisManager.Sites.Remove(site);

                    // Mark the website as non-existant.
                    website.IisSite.SiteID = 0;
                }
            }
            else
            {
                if (website.IisSite.Mode != WebsiteIisMode.Disabled)
                {
                    // Give the site an unused ID (not auto generated by config).
                    site = iisManager.Sites.CreateElement();
                    site.Id = website.IisSite.SiteID;

                    // Apply the neccecary values, then add to config.
                    applySiteValues(website, site);
                    iisManager.Sites.Add(site);

                    // After adding the site, add application, hosts, etc.
                    updateIisSite(website, site);

                    // Commit the creation of site before updating redirect.
                    iisManager.CommitChanges();

                    // Update redirect once the site exists in the XML config.
                    updateIisRedirect(website, site.GetWebConfiguration());
                }
            }

            // Commit changes so redirect update can read from config XML file.
            iisManager.CommitChanges();
        }

        private long getNextIisSiteID()
        {
            return iisManager.Sites.Max(s => s.Id) + 1;
        }

        private void updateIisRedirect(Website website, Configuration configuration)
        {
            ConfigurationSection cs = configuration.GetSection("system.webServer/httpRedirect");
            if (website.IisSite.Mode == WebsiteIisMode.Redirect)
            {
                cs.SetAttributeValue("enabled", true);
                cs.SetAttributeValue("destination", website.IisSite.RedirectUrl);

                // For now, make these static, but they may need to be dynamic in future.
                cs.SetAttributeValue("exactDestination", true);
                cs.SetAttributeValue("childOnly", false);
                cs.SetAttributeValue("httpResponseStatus", 301);
            }
            else
            {
                // Only disable if currently enabled.
                if ((bool)cs.GetAttributeValue("enabled"))
                {
                    // Enable, which creates web.config file.
                    cs.SetAttributeValue("enabled", false);
                }
            }
        }

        private void updateIisSite(Website website, Site site)
        {
            var aq = from a in site.Applications
                     where a.Path == "/"
                     select a;

            Application application;
            if (aq.Count() != 0)
            {
                application = aq.First();
                applyApplicationValues(website, application);
            }
            else
            {
                application = site.Applications.CreateElement();
                application.Path = "/";
                applyApplicationValues(website, application);
                site.Applications.Add(application);
            }

            var vq = from v in application.VirtualDirectories
                     where v.Path == "/"
                     select v;

            VirtualDirectory virtualDirectory;
            if (vq.Count() != 0)
            {
                virtualDirectory = vq.First();
                applyVirtualDirectoryValues(website, virtualDirectory);
            }
            else
            {
                virtualDirectory = application.VirtualDirectories.CreateElement();
                virtualDirectory.Path = "/";
                applyVirtualDirectoryValues(website, virtualDirectory);
                application.VirtualDirectories.Add(virtualDirectory);
            }

            site.Bindings.Clear();
            foreach (WebsiteHost websiteHost in 
                website.HostArray.Where(h => !RhspData.IsDeleteOrDiscard(h)))
            {
                Binding binding = site.Bindings.CreateElement();
                binding.Protocol = websiteHost.GetIisProtocol();
                binding.BindingInformation = websiteHost.GetBindingInformation();
                site.Bindings.Add(binding);
            }

            try
            {
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to commit changes to IIS site.", ex);
            }

            website.IisSite.SiteID = site.Id;
        }

        private void applyVirtualDirectoryValues(
            Website website, 
            VirtualDirectory virtualDirectory)
        {
            virtualDirectory.PhysicalPath = website.GetDirectory(ServerConfig.WebsiteDirectory).FullName;
            virtualDirectory.UserName = website.IisSite.IdentityUserName;
            virtualDirectory.Password = DecryptPassword(website.IisSite.IdentityPassword);
        }

        private void applyApplicationValues(Website website, Application application)
        {
            application.ApplicationPoolName = website.IisSite.ApplicationPoolName;
        }

        private void applySiteValues(Website website, Site site)
        {
            site.Name = website.IisSite.SiteName;
            if (website.IisSite.Mode == WebsiteIisMode.Redirect)
            {
                site.Name += " (Redirect)";
            }
        }

        private void syncronizeDirectory(CleanWebsite website)
        {
            // Website directory may not exist after first install.
            if (!ServerConfig.WebsiteDirectory.Exists)
            {
                ServerConfig.WebsiteDirectory.Create();
            }

            DirectoryInfo customerDirectory = getCustomerDirectory(website);
            DirectoryInfo websiteDirectory = getWebsiteDirectory(website);

            // Only create directory if IIS is not disabled.
            if (website.IisSite.Mode != WebsiteIisMode.Disabled)
            {
                if (!customerDirectory.Exists)
                {
                    customerDirectory.Create();
                }

                if (!websiteDirectory.Exists)
                {
                    websiteDirectory.Create();
                }
            }
        }

        private DirectoryInfo getCustomerDirectory(CleanWebsite website)
        {
            return website.Customer.GetDirectory(ServerConfig.WebsiteDirectory);
        }

        [RhspModuleMethod]
        public string[] GetIpAddressArray()
        {
            return ServerConfig.AvailableIisSiteIPs;
        }

        [RhspModuleMethod]
        public Website[] GetByCustomer(RhspDataID dataID)
        {
            var q = from w in GetAll()
                    where w.CustomerID == dataID
                    select w;

            return q.ToArray();
        }
    }
}
