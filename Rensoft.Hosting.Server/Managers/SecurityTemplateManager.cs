using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Rensoft.ServerManagement.Security;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("SecurityTemplate")]
    public class SecurityTemplateManager : RhspManager
    {
        public void Create(SecurityTemplate securityTemplate)
        {
            enforceConstraints(securityTemplate);
            HostingConfig.Create(securityTemplate);
        }

        public void Update(SecurityTemplate securityTemplate)
        {
            SecurityTemplate existing = Get(securityTemplate.DataID);
            if (existing.Access != securityTemplate.Access)
            {
                throw new Exception("Access mode cannot be changed once set.");
            }

            enforceConstraints(securityTemplate);
            HostingConfig.Update(securityTemplate);
        }

        public void Delete(RhspDataID dataID)
        {
            SecurityTemplate securityTemplate = Get(dataID);
            if (!string.IsNullOrEmpty(securityTemplate.Username))
            {
                // Only try to delete the physical rule if the user is specified.
                WindowsUser windowsUser = findWindowsUser(securityTemplate.Username);
                if (windowsUser != null)
                {
                    // Only try to delete when the user is still on the system.
                    deleteFromFileSystem(securityTemplate, windowsUser.Sid);
                }
            }
            HostingConfig.Delete<SecurityTemplate>(dataID);
        }

        public bool FullPathExists(string fullPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            FileInfo fileInfo = new FileInfo(fullPath);

            return directoryInfo.Exists || fileInfo.Exists;
        }

        public bool UserExists(string username)
        {
            WindowsUserManager wuManager = new WindowsUserManager(ServerConfig.WindowsServerName);
            return wuManager.Exists(username);
        }

        private void enforceConstraints(SecurityTemplate securityTemplate)
        {
            if (!RelativePathExists(securityTemplate.WebsiteID,  securityTemplate.RelativePath))
            {
                string fullPath = getFullPath(
                    securityTemplate.WebsiteID,
                    securityTemplate.RelativePath);

                throw new Exception(
                    "The Windows file system path '" + fullPath + "' does not exist.");
            }

            if (!securityTemplate.UsingIisIdenityWhileIisSiteDisabled())
            {
                if (string.IsNullOrEmpty(securityTemplate.Username))
                {
                    if (securityTemplate.UseIisIdentity)
                    {
                        throw new Exception(
                            "Security template is set to use IIS identity, but " +
                            "the username for this identity is null or empty.");
                    }
                    else
                    {
                        throw new Exception(
                            "The username property of the security template cannot be null.");
                    }
                }

                if (!UserExists(securityTemplate.Username))
                {
                    throw new Exception(
                        "There is no Windows user with username '" + securityTemplate.Username + "'.");
                }
            }
        }

        public bool RelativePathExists(DirectoryInfo websiteDirectory, string relativePath)
        {
            return FullPathExists(getFullPath(websiteDirectory, relativePath));
        }

        public bool RelativePathExists(RhspDataID websiteID, string relativePath)
        {
            return FullPathExists(getFullPath(websiteID, relativePath));
        }

        private string getFullPath(DirectoryInfo websiteDirectory, string relativePath)
        {
            return SecurityTemplate.GetFullPath(
                websiteDirectory,
                relativePath);
        }

        private string getFullPath(RhspDataID websiteID, string relativePath)
        {
            return SecurityTemplate.GetFullPath(
                ServerConfig.WebsiteDirectory,
                CreateManager<WebsiteManager>().Get(websiteID),
                relativePath);
        }

        public SecurityTemplate Get(RhspDataID dataID)
        {
            try
            {
                return getQuery().Where(r => r.DataID == dataID).Single();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get security template with ID '" + dataID + "'.", ex);
            }
        }

        public SecurityTemplate[] GetAll()
        {
            return getQuery().ToArray();
        }

        private IEnumerable<SecurityTemplate> getQuery()
        {
            return from r in HostingConfig.GetArray<SecurityTemplate>()
                   join w in HostingConfig.GetArray<Website>()
                       on r.WebsiteID equals w.DataID
                   join c in HostingConfig.GetArray<Customer>()
                       on w.CustomerID equals c.DataID
                   select SecurityTemplate.Combine(r, Website.Combine(w, c));
        }

        public void Process(SecurityTemplate securityRule)
        {
            switch (securityRule.PendingAction)
            {
                case ChildPendingAction.Create: Create(securityRule); break;
                case ChildPendingAction.Update: Update(securityRule); break;
                case ChildPendingAction.Delete: Delete(securityRule.DataID); break;
            }

            // Path may have been deleted outside of the application.
            if (securityRule.RelativePathExists(ServerConfig.WebsiteDirectory)
                && !securityRule.UsingIisIdenityWhileIisSiteDisabled())
            {
                // Syncronise even if action is none (fs may be out of date).
                SyncronizeFileSystem(securityRule);
            }
        }

        public void SyncronizeFileSystem(SecurityTemplate securityTemplate)
        {
            string fullPath = securityTemplate.GetFullPath(ServerConfig.WebsiteDirectory);
            WindowsUser windowsUser = getWindowsUser(securityTemplate.Username);
            FileInfo fileInfo = new FileInfo(fullPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

            if (fileInfo.Exists)
            {
                FileSecurity security = fileInfo.GetAccessControl();
                applySecurityAccessRule(securityTemplate, windowsUser, security);
                fileInfo.SetAccessControl(security);
            }
            else if (directoryInfo.Exists)
            {
                DirectorySecurity security = directoryInfo.GetAccessControl();
                applySecurityAccessRule(securityTemplate, windowsUser, security);
                directoryInfo.SetAccessControl(security);
            }
            else 
            {
                throw new Exception(
                    "Cannot set security rule because the path '" + fullPath + "' does not exist.");
            }
        }

        private void deleteFromFileSystem(SecurityTemplate securityTemplate, SecurityIdentifier sid)
        {
            string fullPath = securityTemplate.GetFullPath(ServerConfig.WebsiteDirectory);
            FileInfo fileInfo = new FileInfo(fullPath);
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

            // If it exists, delete it, if not, ignore the delete request.
            if (fileInfo.Exists)
            {
                FileSecurity security = File.GetAccessControl(fileInfo.FullName);
                RemoveMatchingRules(security, sid, securityTemplate.GetAccessControlType());
                File.SetAccessControl(directoryInfo.FullName, security);
            }
            else if (directoryInfo.Exists)
            {
                DirectorySecurity security = Directory.GetAccessControl(directoryInfo.FullName);
                RemoveMatchingRules(security, sid, securityTemplate.GetAccessControlType());
                Directory.SetAccessControl(fileInfo.FullName, security);
            }
        }

        private WindowsUser getWindowsUser(string username)
        {
            WindowsUser windowsUser = findWindowsUser(username);
            if (windowsUser == null)
            {
                throw new Exception(
                    "There is no Windows user with username '" + username + "'.");
            }
            return windowsUser;
        }

        private WindowsUser findWindowsUser(string username)
        {
            WindowsUserManager manager = new WindowsUserManager(ServerConfig.WindowsServerName);
            WindowsUser windowsUser = manager.Find(username);
            return windowsUser;
        }

        private IEnumerable<FileSystemAccessRule> getMatchingRules<TSecurity>(
            SecurityIdentifier identity,
            AccessControlType access,
            TSecurity security)
            where TSecurity : FileSystemSecurity
        {
            AuthorizationRuleCollection rules = security.GetAccessRules(
                true, false, typeof(SecurityIdentifier));

            var q = from r in rules.OfType<FileSystemAccessRule>()
                    where r.IdentityReference == identity
                    where r.AccessControlType == access
                    select r;

            return q;
        }

        private void applySecurityAccessRule<TSecurity>(
            SecurityTemplate securityTemplate,
            WindowsUser windowsUser,
            TSecurity security)
            where TSecurity : FileSystemSecurity
        {
            RemoveMatchingRules<TSecurity>(
                security, 
                windowsUser.Sid,
                securityTemplate.GetAccessControlType());

            // Add new rule (effectively replace if removed) to apply security.
            security.AddAccessRule(GetFileSystemAccessRule<TSecurity>(securityTemplate, windowsUser));
        }

        protected void RemoveMatchingRules<TSecurity>(
            TSecurity security,
            SecurityIdentifier sid,
            SecurityTemplate template)
            where TSecurity : FileSystemSecurity
        {
            RemoveMatchingRules<TSecurity>(security, sid, template.GetAccessControlType());
        }

        public void RemoveMatchingRules<TSecurity>(
            TSecurity security,
            SecurityIdentifier sid, 
            AccessControlType accessControlType) 
            where TSecurity : FileSystemSecurity
        {
            var q = getMatchingRules(
                sid,
                accessControlType,
                security);

            if (q.Count() != 0)
            {
                // Remove the existing rule.
                security.RemoveAccessRule(q.First());
            }
        }

        public FileSystemAccessRule GetFileSystemAccessRule<TSecurity>(
            SecurityTemplate template,
            WindowsUser windowsUser)
            where TSecurity : FileSystemSecurity
        {
            if (typeof(TSecurity) == typeof(DirectorySecurity))
            {
                // Directory security with default inheritance (files and subfolders).
                return new FileSystemAccessRule(
                    windowsUser.Sid,
                    template.GetFileSystemRights(),
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    template.GetAccessControlType());
            }
            else if (typeof(TSecurity) == typeof(FileSecurity))
            {
                // Files must not have inheritance specified.
                return new FileSystemAccessRule(
                    windowsUser.Sid,
                    template.GetFileSystemRights(),
                    template.GetAccessControlType());
            }
            else
            {
                throw new NotSupportedException(
                    "File system security type '" + typeof(TSecurity).FullName + "' is not supported.");
            }
        }
    }
}
