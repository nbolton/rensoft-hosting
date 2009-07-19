using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Rensoft.Hosting.Server.Managers.Config;
using System.Xml.Linq;
using Rensoft.Hosting.Server.Managers;
using System.IO;
using Rensoft.ServerManagement.Security;
using System.Security.Principal;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    [RhspData("Website", "Websites", "WebsiteID", SchemaVersion=3)]
    public class Website : RhspData, IRhspData, IRhspDataParent
    {
        [DataMember]
        public RhspDataID CustomerID { get; set; }

        [DataMember]
        public RhspDataID PrimaryHostID { get; set; }

        [DataMember]
        public WebsiteHost[] HostArray { get; set; }

        [DataMember]
        public DnsZone[] DnsZoneArray { get; set; }

        [DataMember]
        public SecurityTemplate[] SecurityArray { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public WebsiteIisSite IisSite { get; set; }

        public WebsiteHost PrimaryHost
        {
            get
            {
                if (PrimaryHostID == null)
                {
                    throw new Exception(
                        "The primary host cannot be found because the primary host ID was not set.");
                }

                return HostArray.Where(h => h.DataID == PrimaryHostID).Single();
            }
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "Website",
                new XElement("CustomerID", CustomerID),
                new XElement("PrimaryHostID", PrimaryHostID),
                new XElement("IisSite",
                    new XElement("IisMode", IisSite.Mode),
                    new XElement("IisSiteID", IisSite.SiteID),
                    new XElement("IisSiteName", IisSite.SiteName),
                    new XElement("IisRedirectUrl", IisSite.RedirectUrl),
                    new XElement("IisIdentitySid", IisSite.IdentitySid),
                    new XElement("IisIdentityUserName", IisSite.IdentityUserName),
                    new XElement("IisIdentityPassword", getEncryptedPassword(IisSite.IdentityPassword)),
                    new XElement("IisApplicationPoolName", IisSite.ApplicationPoolName),
                    new XElement("IisManagedPipelineMode", IisSite.ManagedPipelineMode),
                    new XElement("IisManagedRuntimeVersion", IisSite.ManagedRuntimeVersion)
                )
            );
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            CustomerID = element.GetElementValue<RhspDataID>("CustomerID", false);
            PrimaryHostID = element.GetElementValue<RhspDataID>("PrimaryHostID", true);

            if (manager.GetDataSchemaVersion<Website>(DataID) >= 3)
            {
                fromVersion3IisSite(element);
            }
            else
            {
                fromVersion2IisSite(element);
            }

            if (GetSchemaVersion(manager) == 1)
            {
                // In in the first version of website, hosts are stored within the website structure.
                IEnumerable<XElement> hostElements = element.Element("Hosts").Nodes().Cast<XElement>();
                HostArray = hostElements.Select(e => parseVersion1Host(e, manager)).ToArray();
            }
        }

        private void fromVersion2IisSite(XElement element)
        {
            IisSite = new WebsiteIisSite();
            IisSite.Website = this;
            IisSite.Mode = element.GetElementValue<WebsiteIisMode>("IisMode", false);
            IisSite.SiteID = element.GetElementValue<int>("IisSiteID", true);
            IisSite.SiteName = element.GetElementValue<string>("IisSiteName", true);
            IisSite.RedirectUrl = element.GetElementValue<string>("IisRedirectUrl", true);
            IisSite.IdentitySid = element.GetElementValue<string>("IisIdentitySid", true);
            IisSite.IdentityUserName = element.GetElementValue<string>("IisIdentityUserName", true);
            IisSite.IdentityPassword = getPasswordFromElement(element, "IisIdentityPassword", true);
            IisSite.ApplicationPoolName = element.GetElementValue<string>("IisApplicationPoolName", true);
            IisSite.ManagedPipelineMode = element.GetElementValue<WebsiteIisManagedPipelineMode>("IisManagedPipelineMode", true);
            IisSite.ManagedRuntimeVersion = element.GetElementValue<string>("IisManagedRuntimeVersion", true);
        }

        private void fromVersion3IisSite(XElement element)
        {
            XElement ise = element.Element("IisSite");
            if (ise == null)
            {
                throw new Exception("No element for IisSite was found in the configuration.");
            }

            IisSite = new WebsiteIisSite();
            IisSite.Website = this;
            IisSite.Mode = ise.GetElementValue<WebsiteIisMode>("IisMode", false);
            IisSite.SiteID = ise.GetElementValue<int>("IisSiteID", true);
            IisSite.SiteName = ise.GetElementValue<string>("IisSiteName", true);
            IisSite.RedirectUrl = ise.GetElementValue<string>("IisRedirectUrl", true);
            IisSite.IdentitySid = ise.GetElementValue<string>("IisIdentitySid", true);
            IisSite.IdentityUserName = ise.GetElementValue<string>("IisIdentityUserName", true);
            IisSite.IdentityPassword = getPasswordFromElement(ise, "IisIdentityPassword", true);
            IisSite.ApplicationPoolName = ise.GetElementValue<string>("IisApplicationPoolName", true);
            IisSite.ManagedPipelineMode = ise.GetElementValue<WebsiteIisManagedPipelineMode>("IisManagedPipelineMode", true);
            IisSite.ManagedRuntimeVersion = ise.GetElementValue<string>("IisManagedRuntimeVersion", true);
        }

        private WebsiteHost parseVersion1Host(XElement e, HostingConfigManager manager)
        {
            WebsiteHost host = Activator.CreateInstance<WebsiteHost>();
            host.FromElement(e, manager);
            return host;
        }

        private TChild withParent<TChild>(IRhspDataChild child, IRhspDataParent parent)
            where TChild : IRhspDataChild
        {
            child.Parent = parent;
            return (TChild)child;
        }

        private Password getPasswordFromElement(XElement parent, string elementName, bool allowDefault)
        {
            string encrypted = parent.GetElementValue<string>(elementName, allowDefault);
            if (!string.IsNullOrEmpty(encrypted))
            {
                return Password.FromEncrypted(encrypted);
            }
            else if (allowDefault)
            {
                return default(Password);
            }
            else
            {
                throw new Exception(
                    "Cannot open password because there is no encrypted text.");
            }
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetElementValue("PrimaryHostID", PrimaryHostID);

            // Remove all existing elements from earlier versions nolonger used.
            element.Elements("IisMode").Remove();
            element.Elements("IisSiteID").Remove();
            element.Elements("IisSiteName").Remove();
            element.Elements("IisRedirectUrl").Remove();
            element.Elements("IisIdentitySid").Remove();
            element.Elements("IisIdentityUserName").Remove();
            element.Elements("IisIdentityPassword").Remove();
            element.Elements("IisApplicationPoolName").Remove();
            element.Elements("IisManagedPipelineMode").Remove();
            element.Elements("IisManagedRuntimeVersion").Remove();

            XElement ise;
            var iisSiteQuery = from e in element.Elements() where e.Name == "IisSite" select e;
            if (iisSiteQuery.Count() != 0)
            {
                ise = iisSiteQuery.Single();
            }
            else
            {
                ise = new XElement("IisSite");
                element.Add(ise);
            }

            // Ensure website set for when name is called.
            IisSite.Website = this;

            ise.SetElementValue("IisMode", IisSite.Mode);
            ise.SetElementValue("IisSiteID", IisSite.SiteID);
            ise.SetElementValue("IisSiteName", IisSite.SiteName);
            ise.SetElementValue("IisRedirectUrl", IisSite.RedirectUrl);
            ise.SetElementValue("IisIdentitySid", IisSite.IdentitySid);
            ise.SetElementValue("IisIdentityUserName", IisSite.IdentityUserName);
            ise.SetElementValue("IisIdentityPassword", getEncryptedPassword(IisSite.IdentityPassword));
            ise.SetElementValue("IisApplicationPoolName", IisSite.ApplicationPoolName);
            ise.SetElementValue("IisManagedPipelineMode", IisSite.ManagedPipelineMode.ToString());
            ise.SetElementValue("IisManagedRuntimeVersion", IisSite.ManagedRuntimeVersion);

            // As of version 2, hosts are nolonger within website.
            element.Elements("Hosts").Remove();
        }

        private string getEncryptedPassword(Password password)
        {
            if (password != null)
            {
                return password.GetEncrypted();
            }
            else
            {
                return null;
            }
        }

        //private WebsiteHost[] getHostNameArray(XElement element)
        //{
        //    var q = from hostElement in element.Element("Hosts").Elements("Host")
        //            select new WebsiteHost
        //            {
        //            };

        //    return q.ToArray();
        //}

        //private IEnumerable<XElement> getHostElements()
        //{
        //    return from host in HostArray
        //           select new XElement(
        //               "Host",
        //               new XElement("Name", host.Name),
        //               new XElement("Port", host.Port),
        //               new XElement("IpAddress", host.IpAddress),
        //               new XElement("Primary", host.Primary.ToString())
        //           );
        //}

        public IEnumerable<IRhspDataChild> GetDataChildren()
        {
            List<IRhspDataChild> list = new List<IRhspDataChild>();
            if (HostArray != null)
            {
                list.AddRange(HostArray);
            }
            if (DnsZoneArray != null)
            {
                list.AddRange(DnsZoneArray);
            }
            if (SecurityArray != null)
            {
                list.AddRange(SecurityArray);
            }
            return list;
        }

        public static DirectoryInfo GetDirectory(
            DirectoryInfo baseDirectory, 
            Customer customer,
            string websiteHostName)
        {
            if (customer == null)
            {
                throw new Exception(
                    "Cannot get the website directory because the Customer property has not been set.");
            }

            return new DirectoryInfo(
                Path.Combine(customer.GetDirectory(baseDirectory).FullName, websiteHostName)
            );
        }

        public DirectoryInfo GetDirectory(DirectoryInfo baseDirectory)
        {
            return GetDirectory(baseDirectory, Customer, PrimaryHost.Name);
        }

        internal static Website Combine(Website w, Customer c)
        {
            w.Customer = c;
            return w;
        }

        public WindowsUser GetIisIdentity(RhspManager manager)
        {
            if (IisSite.IdentityUserName == null)
            {
                throw new Exception("The IIS identity user name has not been set.");
            }

            WindowsUser windowsUser = new WindowsUser(
                IisSite.IdentityUserName,
                manager.DecryptPassword(IisSite.IdentityPassword),
                "IIS Identity (" + PrimaryHost.Name + ")",
                "User for the IIS site " + PrimaryHost.Name + " and it's application pool.",
                WindowsUserFlag.PasswordCannotChange | WindowsUserFlag.PasswordNeverExpires,
                new WindowsUserGroup("IIS_IUSRS"));

            if (!string.IsNullOrEmpty(IisSite.IdentitySid))
            {
                windowsUser.Sid = new SecurityIdentifier(IisSite.IdentitySid);
            }

            return windowsUser;
        }

        internal void TakeCleanValues(CleanWebsite cleanWebsite)
        {
            IisSite.IdentityPassword = cleanWebsite.IisSite.IdentityPassword;
            IisSite.IdentitySid = cleanWebsite.IisSite.IdentitySid;
            IisSite.SiteID = cleanWebsite.IisSite.SiteID;
        }

        public void GenerateIisPassword(RhspManager manager)
        {
            IisSite.IdentityPassword = manager.EncryptPassword(RsRandom.GenerateString(14));
        }
    }
}
