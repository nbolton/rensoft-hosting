using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Managers.Config;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Security.AccessControl;
using Rensoft.ServerManagement.Security;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    [RhspData("SecurityRule", "SecurityRules", "SecurityRuleID")]
    public class SecurityTemplate : RhspData, IRhspData, IRhspDataChild
    {
        private string username;

        public RhspDataID ParentID
        {
            get { return WebsiteID; }
            set { WebsiteID = value; }
        }

        [DataMember]
        public RhspDataID WebsiteID { get; set; }

        [DataMember]
        public string RelativePath { get; set; }

        [DataMember]
        public string Username
        {
            get
            {
                if (UseIisIdentity)
                {
                    assertWebsiteSet();
                    return Website.IisSite.IdentityUserName;
                }
                else
                {
                    return username;
                }
            }
            set
            {
                if (!UseIisIdentity)
                {
                    username = value;
                }
                else
                {
                    username = null;
                }
            }
        }

        private void assertWebsiteSet()
        {
            if (Website == null)
            {
                throw new Exception("Website has not been set.");
            }
        }

        [DataMember]
        public bool Read { get; set; }

        [DataMember]
        public bool Write { get; set; }

        [DataMember]
        public bool Delete { get; set; }

        [DataMember]
        public SecurityTemplateAccess Access { get; set; }

        [DataMember]
        public ChildPendingAction PendingAction { get; set; }

        [DataMember]
        public bool UseIisIdentity { get; set; }

        public Website Website { get; set; }

        public IRhspDataParent Parent
        {
            get { return Website; }
            set { Website = (Website)value; }
        }

        public SecurityTemplate() { }

        public static SecurityTemplate Combine(SecurityTemplate rule, Website website)
        {
            rule.Website = website;
            return rule;
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "SecurityRule",
                new XElement("WebsiteID", WebsiteID),
                new XElement("RelativePath", RelativePath),
                new XElement("Username", Username),
                new XElement("Read", Read),
                new XElement("Write", Write),
                new XElement("Delete", Delete),
                new XElement("Access", Access.ToString()),
                new XElement("UseIisIdentity", UseIisIdentity)
            );
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            WebsiteID = new RhspDataID(element.GetElementValue<string>("WebsiteID", false));
            RelativePath = element.GetElementValue<string>("RelativePath", false);
            Username = element.GetElementValue<string>("Username", true);
            Read = element.GetElementValue<bool>("Read", false);
            Write = element.GetElementValue<bool>("Write", false);
            Delete = element.GetElementValue<bool>("Delete", false);
            Access = parseAccess(element.GetElementValue<string>("Access", false));
            UseIisIdentity = element.GetElementValue<bool>("UseIisIdentity", true);
        }

        private SecurityTemplateAccess parseAccess(string value)
        {
            return (SecurityTemplateAccess)Enum.Parse(typeof(SecurityTemplateAccess), value);
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetElementValue("Read", Read);
            element.SetElementValue("Write", Write);
            element.SetElementValue("Delete", Delete);
            element.SetElementValue("Access", Access);
            element.SetElementValue("Username", Username);
            element.SetElementValue("UseIisIdentity", UseIisIdentity);
        }

        public string GetFullPath(DirectoryInfo websiteBaseDirectory)
        {
            return GetFullPath(websiteBaseDirectory, Website, RelativePath);
        }

        public static string GetFullPath(
            DirectoryInfo websiteBaseDirectory,
            Website website,
            string relativePath)
        {
            if (website == null)
            {
                throw new Exception(
                    "Cannot get full path when website is not set.");
            }

            DirectoryInfo websiteDirectory = website.GetDirectory(websiteBaseDirectory);
            return GetFullPath(websiteDirectory, relativePath);
        }

        public static string GetFullPath(
            DirectoryInfo websiteDirectory,
            string relativePath)
        {
            string cleanRP = relativePath.Replace('/', Path.DirectorySeparatorChar);

            if (cleanRP.StartsWith(Path.DirectorySeparatorChar.ToString()))
            {
                cleanRP = cleanRP.Remove(0, 1);
            }

            return Path.Combine(websiteDirectory.FullName, cleanRP);
        }

        public AccessControlType GetAccessControlType()
        {
            switch (Access)
            {
                case SecurityTemplateAccess.Allow:
                    return AccessControlType.Allow;

                case SecurityTemplateAccess.Deny:
                    return AccessControlType.Deny;

                default:
                    throw new NotSupportedException(
                        "The access type '" + Access.ToString() + "' is not supported.");
            }
        }

        public FileSystemRights GetFileSystemRights()
        {
            FileSystemRights rights = (FileSystemRights)0;
            if (Read)
            {
                rights |= FileSystemRights.Read | FileSystemRights.Traverse;
            }
            if (Write)
            {
                rights |= FileSystemRights.Write;
            }
            if (Delete)
            {
                rights |= FileSystemRights.Delete;
            }
            return rights;
        }

        internal bool RelativePathExists(DirectoryInfo websiteDirectory)
        {
            string fullPath = GetFullPath(websiteDirectory);
            FileInfo fileInfo = new FileInfo(fullPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            return fileInfo.Exists || directoryInfo.Exists;
        }

        internal bool UsingIisIdenityWhileIisSiteDisabled()
        {
            assertWebsiteSet();
            return (Website.IisSite.Mode == WebsiteIisMode.Disabled) && UseIisIdentity;
        }
    }
}
