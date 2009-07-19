using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using System.IO;
using System.Security.AccessControl;
using Rensoft.ServerManagement.Security;
using Microsoft.Web.Administration;
using System.Security.Principal;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("Customer")]
    public class CustomerManager : RhspManager
    {
        private FileZillaManager ftpManager;
        //private WindowsUserManager wuManager;
        //private ServerManager iisManager;

        public DirectoryInfo BaseWebsiteDirectory
        {
            get { return Context.ServerConfig.WebsiteDirectory; }
        }

        public CustomerManager()
        {
            Load += new EventHandler(CustomerManager_Load);
        }

        void CustomerManager_Load(object sender, EventArgs e)
        {
            ftpManager = CreateManager<FileZillaManager>();
            //wuManager = new WindowsUserManager(ServerConfig.WindowsServerName);
            //iisManager = new ServerManager();
        }

        [RhspModuleMethod]
        public void Create(Customer customer)
        {
            enforceUniqueCode(customer);
            Context.HostingConfig.Create(customer);
            //setReadOnlyValues(customer);
            syncronizeHosting(customer);
        }

        //private void setReadOnlyValues(Customer customer)
        //{
        //    XDocument document = HostingConfig.LoadConfigDocument();
        //    XElement element = HostingConfig.GetElement<Customer>(document, customer.DataID);
        //    customer.SetReadOnlyValues(element, this);
        //    HostingConfig.SaveConfigDocument(document);
        //}

        [RhspModuleMethod]
        public void Update(Customer customer)
        {
            enforceUniqueCode(customer);
            Context.HostingConfig.Update(customer);
            syncronizeHosting(customer);
        }

        [RhspModuleMethod]
        public CustomerDeleteResult Delete(RhspDataID dataID)
        {
            CustomerDeleteResult r = new CustomerDeleteResult();

            // Get the customer to delete physical objects.
            Customer customer = Get(dataID);

            // Delete websites belonging to customer.
            WebsiteManager wm = CreateManager<WebsiteManager>();
            wm.GetByCustomer(dataID).ToList().ForEach(
                w => r.Errors.AddRange(wm.Delete(w.DataID).Errors));

            // Make client directory obsolete after websites are made obsolete.
            MakeDirectoryObsoleteResult mdor = TryMakeDirectoryObsolete(getDirectory(customer));
            if (mdor.Error != null)
            {
                r.Errors.Add(mdor.Error.Message);
            }

            // Delete ftp users, group, and security.
            deleteFtpObjects(customer);

            // Finally delete the actual customer configuration.
            Context.HostingConfig.Delete<Customer>(dataID);

            return r;
        }

        private void deleteFtpObjects(Customer customer)
        {
            /* Deleting group also deletes users, and because security permissions
             * are stored within groups and users, these are also deleted. */
            CreateManager<FileZillaManager>().DeleteGroup(customer.FtpGroupName, true);
        }

        private DirectoryInfo getDirectory(Customer customer)
        {
            return customer.GetDirectory(ServerConfig.WebsiteDirectory);
        }

        private void syncronizeHosting(Customer customer)
        {
            syncronizeDirectory(customer);
            syncronizeFtpServer(customer);
            //syncronizeWindowsUsers(customer.DataID);
            //syncronizeIisAppPool(customer.DataID);
        }

        //private void syncronizeWindowsUsers(HostingDataID dataID)
        //{
        //    Customer customer = Get(dataID);
        //    XDocument document = HostingConfig.LoadConfigDocument();
        //    XElement element = HostingConfig.GetElement<Customer>(document, customer.DataID);

        //    if (!wuManager.Exists(customer.PublicWebsiteUserName))
        //    {
        //        SecurityIdentifier sid = wuManager.Create(customer.GetPublicWebsiteUser(this));
        //        element.SetElementValue("PublicWebsiteUserSid", sid.Value);
        //    }

        //    if (!wuManager.Exists(customer.WorkerProcessUserName))
        //    {
        //        SecurityIdentifier sid = wuManager.Create(customer.GetWorkerProcessUser(this));
        //        element.SetElementValue("WorkerProcessUserSid", sid.Value);
        //    }

        //    HostingConfig.SaveConfigDocument(document);
        //}

        //private void syncronizeIisAppPool(HostingDataID dataID)
        //{
        //    Customer customer = Get(dataID);

        //    var q = from a in iisManager.ApplicationPools
        //            where a.Name == customer.ApplicationPoolName
        //            select a;

        //    ApplicationPool ap;
        //    if (q.Count() != 0)
        //    {
        //        ap = q.First();
        //    }
        //    else
        //    {
        //        ap = iisManager.ApplicationPools.Add(
        //            customer.ApplicationPoolName);
        //    }

        //    ap.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
        //    ap.ProcessModel.UserName = customer.WorkerProcessUserName;
        //    ap.ProcessModel.Password = DecryptPassword(customer.WorkerProcessPassword);

        //    iisManager.CommitChanges();
        //}

        private void processFtpUserArray(Customer customer)
        {
            if (customer.FtpUserArray != null)
            {
                customer.FtpUserArray.ToList().ForEach(ftpUser => processFtpUser(customer, ftpUser));
            }
        }

        private void processFtpUser(Customer customer, FtpUser ftpUser)
        {
            switch (ftpUser.PendingAction)
            {
                case ChildPendingAction.Create:
                    createFtpUser(customer, ftpUser);
                    updateWebsiteSecurity(customer);
                    break;

                case ChildPendingAction.Update:
                    updateFtpUser(ftpUser);
                    updateWebsiteSecurity(customer);
                    break;

                case ChildPendingAction.Delete: 
                    deleteFtpUser(ftpUser);
                    break;
            }
        }

        private void updateWebsiteSecurity(Customer customer)
        {
            foreach (Website website in GetWebsites(customer.DataID))
            {
                CreateManager<WebsiteManager>().SetDefaultWebsiteSecurity(website);
            }
        }

        private void createFtpUser(Customer customer, FtpUser ftpUser)
        {
            ftpManager.CreateUser(
                ftpUser.UserName,
                DecryptPassword(ftpUser.Password),
                ftpUser.Enabled,
                customer.FtpGroupName);
        }

        public Website[] GetWebsites(RhspDataID customerID)
        {
            return CreateManager<WebsiteManager>().GetByCustomer(customerID);
        }

        private void updateFtpUser(FtpUser ftpUser)
        {
            ftpManager.UpdateUser(ftpUser.UserName, ftpUser.Enabled);
        }

        private void deleteFtpUser(FtpUser ftpUser)
        {
            ftpManager.DeleteUser(ftpUser.UserName);
        }

        private void syncronizeFtpServer(Customer customer)
        {
            bool groupExists = ftpManager.GroupExists(customer.FtpGroupName);

            // Where customer enabled FTP, and it doesnt already exist.
            if (customer.FtpEnabled && !groupExists)
            {
                createFtpGroup(customer);
                groupExists = true;
            }

            // Delete when the group exists, but it shouldnt be enabled.
            if (!customer.FtpEnabled && groupExists)
            {
                ftpManager.DeleteGroup(customer.FtpGroupName, true);
                groupExists = false;
            }

            if (groupExists)
            {
                // Only process when group exists.
                processFtpUserArray(customer);
            }
        }

        private void createFtpGroup(Customer customer)
        {
            DirectoryInfo homeDirectory = customer.GetDirectory(BaseWebsiteDirectory);

            // Create the FTP group to which all users will be added.
            ftpManager.CreateGroup(customer.FtpGroupName, homeDirectory);

            // Don't allow users to create anything in base directory.
            FtpPermissions basePerms = new FtpPermissions();
            basePerms.DirectoryOpen = true;
            basePerms.DirectoryList = true;
            ftpManager.SetGroupPermissions(customer.FtpGroupName, homeDirectory, basePerms);
        }

        [RhspModuleMethod]
        public Customer Get(RhspDataID dataID)
        {
            return Context.HostingConfig.GetSingle<Customer>(dataID);
        }

        [RhspModuleMethod]
        public Customer[] GetAll()
        {
            return Context.HostingConfig.GetArray<Customer>().OrderBy(c => c.Code).ToArray();
        }

        private void syncronizeDirectory(Customer customer)
        {
            // If directory doesn't already exist, and it is needed by FTP, then create.
            DirectoryInfo info = customer.GetDirectory(BaseWebsiteDirectory);
            if (!info.Exists && customer.FtpEnabled)
            {
                info.Create();
            }
        }

        private bool hasWebsites(RhspDataID customerID)
        {
            return CreateManager<WebsiteManager>().GetByCustomer(customerID).Count() != 0;
        }

        [RhspModuleMethod]
        public bool ExistsWithCode(Customer checkCustomer)
        {
            var q = from customer in GetAll()
                    where !customer.DataID.Equals(checkCustomer.DataID)
                    where customer.Code == checkCustomer.Code
                    select customer;

            return q.Count() != 0;
        }

        private void enforceUniqueCode(Customer customer)
        {
            if (ExistsWithCode(customer))
            {
                throw new Exception("Customer already exists with code '" + customer.Code + "'.");
            }
        }
    }
}
