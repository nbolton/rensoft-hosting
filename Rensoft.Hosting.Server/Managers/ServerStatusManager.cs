using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Rensoft.ServerManagement.Security;
using System.ServiceProcess;
using System.Diagnostics;
using Rensoft.Hosting.Server.Properties;
using Rensoft.Hosting.IscBind;
using Rensoft.ServiceProcess;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("ServerStatus")]
    public class ServerStatusManager : RhspManager
    {
        private const string iscBindUser = "named";
        private const string iscBindService = "named";

        [RhspModuleMethod]
        public ServerStatusElement[] GetElementArray()
        {
            List<ServerStatusElement> elementList = new List<ServerStatusElement>();
            elementList.Add(getIscBindSecurityStatus());
            elementList.Add(getIscBindServiceStatus());
            elementList.Add(getIscBindConfigStatus());
            return elementList.ToArray();
        }

        [RhspModuleMethod]
        public ServerStatusActionResult RepairBindConfig()
        {
            WindowsUser namedUser = findWindowsUser();
            if (namedUser == null)
            {
                throw new Exception(
                    "Cannot repair when user '" + iscBindUser + "' does not exist.");
            }

            string iscBindDirectory = ServerConfig.IscBindDirectory.FullName;
            IscBindSetup bindSetup = new IscBindSetup(iscBindDirectory, namedUser.Sid);

            // Backup existing configs and replace with new one.
            bindSetup.InitializeConfig();

            // Regenerate zone config file in case it's corrupt.
            IscBindManager bindManager = CreateManager<IscBindManager>();
            bindManager.RegenerateMainConfigFile();

            ServiceController sc = getIscBindServiceController();
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            }

            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));

            ServerStatusActionResult result = new ServerStatusActionResult();
            if (sc.Status == ServiceControllerStatus.Running)
            {
                result.Success = true;
                result.UserMessage = "The configuration was repaired, and the ISC BIND service was restarted.";
            }
            else
            {
                result.Success = false;
                if (getIscBindConfigStatus().Condition == ServerStatusCondition.Normal)
                {
                    result.UserMessage = "The repair was successful, but the ISC BIND service did not start.";
                }
                else
                {
                    result.UserMessage = "The repair has failed, and the ISC BIND service failed to start.";
                }
            }

            return result;
        }

        private WindowsUser findWindowsUser()
        {
            WindowsUserManager wuManager = new WindowsUserManager(ServerConfig.WindowsServerName);
            WindowsUser namedUser = wuManager.Find(iscBindUser);
            return namedUser;
        }

        private ServerStatusElement getIscBindConfigStatus()
        {
            ServerStatusElement e = new ServerStatusElement();
            e.Name = "ISC BIND configuration status";

            string iscBindDirectory = ServerConfig.IscBindDirectory.FullName;
            string rndcConfPath = Path.Combine(iscBindDirectory, @"etc\rndc.conf");
            string namedConfPath = Path.Combine(iscBindDirectory, @"etc\named.conf");
            string namedConfGenPath = Path.Combine(iscBindDirectory, @"etc\named.generated.conf");

            if (!File.Exists(rndcConfPath))
            {
                e.Value = "File '" + rndcConfPath + "' missing";
                e.Condition = ServerStatusCondition.Error;
            }
            else if (!File.Exists(namedConfPath))
            {
                e.Value = "File '" + namedConfPath + "' missing";
                e.Condition = ServerStatusCondition.Error;
            }
            else if (!File.Exists(namedConfGenPath))
            {
                e.Value = "File '" + namedConfGenPath + "' missing";
                e.Condition = ServerStatusCondition.Error;
            }
            else if (!fileContainsText(namedConfPath, "include \"named.generated.conf\""))
            {
                e.Value = "Generated config include missing from 'named.conf'";
                e.Condition = ServerStatusCondition.Error;
            }
            else
            {
                e.Value = "All files exist";
                e.Condition = ServerStatusCondition.Normal;
            }

            if (e.Condition == ServerStatusCondition.Error)
            {
                e.ActionText = "Repair";
                e.ActionCommand = "RepairBindConfig";
            }

            return e;
        }

        private bool fileContainsText(string path, string text)
        {
            return File.ReadAllText(path).Contains(text);
        }

        private ServerStatusElement getIscBindServiceStatus()
        {
            ServerStatusElement e = new ServerStatusElement();
            e.Name = "ISC BIND service status";

            if (iscBindServiceExists())
            {
                ServiceController sc = getIscBindServiceController();
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    e.Value = "Started";
                    e.Condition = ServerStatusCondition.Normal;
                }
                else
                {
                    e.Value = sc.Status.ToString();
                    e.Condition = ServerStatusCondition.Error;

                    e.ActionText = "Start";
                    e.ActionCommand = "StartBindService";
                }
            }
            else
            {
                e.Value = "Not installed";
                e.Condition = ServerStatusCondition.Error;
            }
            return e;
        }

        private bool iscBindServiceExists()
        {
 	        return (from s in ServiceController.GetServices()
                    where s.ServiceName == iscBindService
                    select s).Count() != 0;
        }

        [RhspModuleMethod]
        public ServerStatusActionResult RepairBindSecurity()
        {
            WindowsUser namedUser = findWindowsUser();
            if (namedUser == null)
            {
                throw new Exception(
                    "Cannot repair when user '" + iscBindUser + "' does not exist.");
            }

            IscBindSetup setup = new IscBindSetup(
                ServerConfig.IscBindDirectory.FullName,
                namedUser.Sid);

            setup.IntitializeSecurity();

            ServerStatusActionResult r = new ServerStatusActionResult();
            if (getIscBindSecurityStatus().Condition == ServerStatusCondition.Normal)
            {
                r.Success = true;
                r.UserMessage = "Security has been repaired.";
            }
            else
            {
                r.Success = false;
                r.UserMessage = "Security was not repaired.";
            }
            return r;
        }

        [RhspModuleMethod]
        public ServerStatusActionResult StartBindService()
        {
            ServiceController sc = getIscBindServiceController();
            sc.Start();
            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));

            ServerStatusActionResult r = new ServerStatusActionResult();
            if (sc.Status == ServiceControllerStatus.Running)
            {
                r.Success = true;
                r.UserMessage = "ISC BIND service started.";
            }
            else
            {
                r.Success = false;
                r.UserMessage = "ISC BIND service failed to start.";
            }
            return r;
        }

        private ServiceController getIscBindServiceController()
        {
            ServiceController sc = new ServiceController(iscBindService);
            return sc;
        }

        private ServerStatusElement getIscBindSecurityStatus()
        {
            ServerStatusElement e = new ServerStatusElement();
            e.Name = "ISC BIND security (" + ServerConfig.IscBindDirectory.FullName + ")";

            DirectorySecurity security = ServerConfig.IscBindDirectory.GetAccessControl();
            AuthorizationRuleCollection rules = security.GetAccessRules(
                true, false, typeof(SecurityIdentifier));

            WindowsUserManager wuManager = new WindowsUserManager(ServerConfig.WindowsServerName);
            WindowsUser namedUser = wuManager.Find(iscBindUser);

            if (namedUser == null)
            {
                e.Value = "Windows user '" + iscBindUser + "' is missing";
                e.Condition = ServerStatusCondition.Error;
            }
            else
            {
                var q = from r in rules.OfType<FileSystemAccessRule>()
                        where r.IdentityReference == namedUser.Sid
                        where r.AccessControlType == AccessControlType.Allow
                        select r;

                if (q.Count() != 0)
                {
                    if ((q.Single().FileSystemRights & FileSystemRights.Modify) == FileSystemRights.Modify)
                    {
                        e.Value = "User '" + iscBindUser + "' can modify";
                        e.Condition = ServerStatusCondition.Normal;
                    }
                    else
                    {
                        e.Value = "User '" + iscBindUser + "' cannot modify";
                        e.Condition = ServerStatusCondition.Error;
                    }
                }
                else
                {
                    e.Value = "User '" + iscBindUser + "' does not have any access";
                    e.Condition = ServerStatusCondition.Error;
                }

                if (e.Condition == ServerStatusCondition.Error)
                {
                    // At this point, if the user exists but the security is wrong, it can be reset.
                    e.ActionText = "Repair";
                    e.ActionCommand = "RepairBindSecurity";
                }
            }

            return e;
        }
    }
}
