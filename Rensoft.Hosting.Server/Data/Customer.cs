using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Rensoft.Hosting.Server.Managers.Config;
using System.Xml.Linq;
using System.IO;
using Rensoft.ServerManagement.Security;
using Microsoft.Web.Administration;
using Rensoft.Hosting.Server.Managers;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    [RhspData("Customer", "Customers", "CustomerID")]
    public class Customer : RhspData, IRhspData
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool FtpEnabled { get; set; }

        [DataMember]
        public FtpUser[] FtpUserArray { get; set; }

        //[DataMember]
        //public string PublicWebsiteUserName { get; set; }

        //[DataMember]
        //public Password PublicWebsitePassword { get; set; }

        //[DataMember]
        //public string PublicWebsiteUserSid { get; set; }

        //[DataMember]
        //public string WorkerProcessUserName { get; set; }

        //[DataMember]
        //public Password WorkerProcessPassword { get; set; }

        //[DataMember]
        //public string WorkerProcessUserSid { get; set; }

        //[DataMember]
        //public string ApplicationPoolName { get; set; }

        public string FtpGroupName
        {
            get { return Code; }
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            this.Code = element.GetElementValue<string>("Code", false);
            this.Name = element.GetElementValue<string>("Name", false);
            this.FtpEnabled = element.GetElementValue<bool>("FtpEnabled", true);
            //this.PublicWebsiteUserName = element.GetElementValue<string>("PublicWebsiteUserName", true);
            //this.PublicWebsitePassword = Password.FromEncrypted(element.GetElementValue<string>("PublicWebsitePassword", true));
            //this.PublicWebsiteUserSid = element.GetElementValue<string>("PublicWebsiteUserSid", true);
            //this.WorkerProcessUserName = element.GetElementValue<string>("WorkerProcessUserName", true);
            //this.WorkerProcessPassword = Password.FromEncrypted(element.GetElementValue<string>("WorkerProcessPassword", true));
            //this.WorkerProcessUserSid = element.GetElementValue<string>("WorkerProcessUserSid", true);
            //this.ApplicationPoolName = element.GetElementValue<string>("ApplicationPoolName", true);
            this.FtpUserArray = getFtpUserArray(element, manager);
        }

        private FtpUser[] getFtpUserArray(XElement element, HostingConfigManager manager)
        {
            if (element.Element("FtpUsers") == null)
            {
                return new FtpUser[0];
            }
            else
            {
                var q = from e in element.Element("FtpUsers").Elements()
                        select FtpUser.CreateFromElement(e, manager);

                return q.ToArray();
            }
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "Customer",
                new XElement("Code", Code.ToUpper()),
                new XElement("Name", Name),
                new XElement("FtpEnabled", FtpEnabled),
                new XElement("FtpUsers",
                    from ftpUser in (FtpUserArray == null ? new FtpUser[0] : FtpUserArray)
                    where ftpUser.PendingAction != ChildPendingAction.Delete
                    select ftpUser.ToXElement(manager)
                )
            );
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            //this.PublicWebsiteUserName = element.GetElementValue<string>("PublicWebsiteUserName", true);
            //this.PublicWebsitePassword = Password.FromEncrypted(element.GetElementValue<string>("PublicWebsitePassword", true));
            //this.PublicWebsiteUserSid = element.GetElementValue<string>("PublicWebsiteUserSid", true);
            //this.WorkerProcessUserName = element.GetElementValue<string>("WorkerProcessUserName", true);
            //this.WorkerProcessPassword = Password.FromEncrypted(element.GetElementValue<string>("WorkerProcessPassword", true));
            //this.WorkerProcessUserSid = element.GetElementValue<string>("WorkerProcessUserSid", true);
            //this.ApplicationPoolName = element.GetElementValue<string>("ApplicationPoolName", true);

            // Remove all existing elements from earlier versions nolonger used.
            element.Elements("PublicWebsiteUserName").Remove();
            element.Elements("PublicWebsitePassword").Remove();
            element.Elements("PublicWebsiteUserSid").Remove();
            element.Elements("WorkerProcessUserName").Remove();
            element.Elements("WorkerProcessPassword").Remove();
            element.Elements("WorkerProcessUserSid").Remove();
            element.Elements("ApplicationPoolName").Remove();

            element.SetElementValue("Name", Name);
            element.SetElementValue("FtpEnabled", FtpEnabled);

            if (FtpUserArray != null)
            {
                FtpUserArray.ToList().ForEach(ftpUser => processFtpUser(ftpUser, element, manager));
            }
        }

        //public void SetReadOnlyValues(XElement element, RhspManager manager)
        //{
        //    ApplicationPoolName = Code + " Application Pool";
        //    element.SetElementValue("ApplicationPoolName", ApplicationPoolName);

        //    PublicWebsiteUserName = Code + "-PublicWebsite";
        //    PublicWebsitePassword = manager.EncryptPassword(RsRandom.GenerateString(14));
        //    WorkerProcessUserName = Code + "-WorkerProcess";
        //    WorkerProcessPassword = manager.EncryptPassword(RsRandom.GenerateString(14));

        //    element.SetElementValue("PublicWebsiteUserName", PublicWebsiteUserName);
        //    element.SetElementValue("PublicWebsitePassword", PublicWebsitePassword.GetEncrypted());
        //    element.SetElementValue("WorkerProcessUserName", WorkerProcessUserName);
        //    element.SetElementValue("WorkerProcessPassword", WorkerProcessPassword.GetEncrypted());
        //}

        private void processFtpUser(FtpUser ftpUser, XElement customerElement, HostingConfigManager manager)
        {
            switch (ftpUser.PendingAction)
            {
                case ChildPendingAction.Create:
                    customerElement.Element("FtpUsers").Add(ftpUser.ToXElement(manager));
                    break;

                case ChildPendingAction.Update:
                    ftpUser.UpdateElement(getFtpUserElement(customerElement, ftpUser.UserName), manager);
                    break;

                case ChildPendingAction.Delete:
                    getFtpUserElement(customerElement, ftpUser.UserName).Remove();
                    break;
            }
        }

        private XElement getFtpUserElement(XElement customerElement, string userName)
        {
            var q = from e in customerElement.Element("FtpUsers").Elements()
                    where e.Element("UserName").Value == userName
                    select e;

            return q.First();
        }

        public DirectoryInfo GetDirectory(DirectoryInfo baseDirectory)
        {
            return new DirectoryInfo(Path.Combine(baseDirectory.FullName, Code));
        }

        //public WindowsUser GetPublicWebsiteUser(RhspManager manager)
        //{
        //    return new WindowsUser(
        //        PublicWebsiteUserName,
        //        manager.DecryptPassword(PublicWebsitePassword),
        //        "IIS Public Website User",
        //        "Public website user for " + Name,
        //        WindowsUserFlag.PasswordCannotChange | WindowsUserFlag.PasswordNeverExpires);
        //}

        //public WindowsUser GetWorkerProcessUser(RhspManager manager)
        //{
        //    return new WindowsUser(
        //        WorkerProcessUserName,
        //        manager.DecryptPassword(WorkerProcessPassword),
        //        "IIS Worker Process User",
        //        "Worker process user for " + Name,
        //        WindowsUserFlag.PasswordCannotChange | WindowsUserFlag.PasswordNeverExpires,
        //        new WindowsUserGroup("IIS_IUSRS"));
        //}
    }
}
