using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.ServiceProcess;
using Rensoft.ServiceProcess;
using Rensoft.ServerManagement.Security;
using Rensoft.Hosting.IscBind;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.Setup.Options
{
    class IscBindSetupOption : SetupOption
    {
        private const string accountUserName = "named";
        private const string accountDisplayName = "ISC BIND";
        private const string accountDescription = "Service account for ISC BIND";
        private const string serviceProcessName = "named";
        private const string serviceProcessBin = "named.exe";
        private const string serviceDisplayName = "ISC BIND";
        private const string serviceDescription = "Allows DNS clients to resolve domain names.";
        private const string rootDirectoryName = "ISC BIND";
        private const string binDirectoryName = "bin";
        private const string etcDirectoryName = "etc";
        private const string uninstallDisplayName = "ISC BIND";

        private bool autoEnabled;
        private string accountPassword;
        private WindowsUser windowsUser;

        public override string ProductName
        {
            get { return "BIND DNS Server 9.5.0-P2-W2"; }
        }

        public override string SetupFileName
        {
            get { return "BIND9.5.0-P2-W2.zip"; }
        }

        public override string RegistryKeyName
        {
            get { return "ISC BIND"; }
        }

        public IscBindSetupOption(SetupWizardForm setupWizard)
            : base(setupWizard)
        {
            // Generate a password without backslashes as to not disrupt command.
            accountPassword = RsRandom.GenerateString(100, new char[] { '\\', '"' });
        }

        public override void Load()
        {
            if (!autoEnabled)
            {
                // Do not suggest installation when already installed.
                Enabled = !IsInstalled();
                autoEnabled = true;
            }
        }

        public override bool ProcessOption(UpdateStatusDelegate updateStatus)
        {
            if (!base.ProcessOption(updateStatus))
            {
                return false;
            }

            if (IsInstalled())
            {
                if (!uninstall(updateStatus))
                {
                    return false;
                }
            }

            try
            {
                return install(updateStatus);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to install components.", ex);
            }
        }

        private bool install(UpdateStatusDelegate updateStatus)
        {
            updateStatus(this, SetupStatus.InProgress, "Extracting files...");
            extractProgramFiles();

            updateStatus(this, SetupStatus.InProgress, "Registering application...");
            registerApplication();

            updateStatus(this, SetupStatus.InProgress, "Installing C++ runtime...");
            installCppRuntime();

            updateStatus(this, SetupStatus.InProgress, "Creating service account...");
            createServiceAccount();

            updateStatus(this, SetupStatus.InProgress, "Registering application...");
            initializeSecurity();

            updateStatus(this, SetupStatus.InProgress, "Initializing configuration...");
            initializeConfig();

            updateStatus(this, SetupStatus.InProgress, "Creating service...");
            int createServiceErrorCode;
            if (!createService(out createServiceErrorCode))
            {
                updateStatus(
                    this, SetupStatus.Failure, 
                    "Could not create service (Error code: " + createServiceErrorCode + ")");
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Starting service...");
            if (!startService())
            {
                updateStatus(this, SetupStatus.Failure, "Could not start service");
                return false;
            }

            if (!IsInstalled())
            {
                updateStatus(this, SetupStatus.Failure, "Installation failed");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void installCppRuntime()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine(getBinPath(), "vcredist_x86.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
        }

        private void initializeConfig()
        {
            IscBindSetup setup = getBindSetup();
            setup.InitializeConfig();
        }

        private void initializeSecurity()
        {
            IscBindSetup setup = getBindSetup();
            setup.IntitializeSecurity();
        }

        private IscBindSetup getBindSetup()
        {
            if (windowsUser == null)
            {
                throw new Exception(
                    "Cannot get bind setup helper because the windows user has not been set.");
            }

            IscBindSetup setup = new IscBindSetup(
                getRootPath(), windowsUser.Sid);
            return setup;
        }

        private bool uninstall(UpdateStatusDelegate updateStatus)
        {
            updateStatus(this, SetupStatus.InProgress, "Stopping service...");
            if (!stopService())
            {
                updateStatus(this, SetupStatus.Failure, "Could not stop service");
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Removing service...");
            if (!removeService())
            {
                updateStatus(this, SetupStatus.Failure, "Could not remove service");
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Removing service account...");
            if (!removeServiceAccount())
            {
                updateStatus(this, SetupStatus.Failure, "Could not remove service account");
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Deleting files...");
            deleteProgramFiles();

            updateStatus(this, SetupStatus.InProgress, "Unregistering application...");
            unregisterApplication();

            return true;
        }

        private void registerApplication()
        {
            RegistryKey uninstallKey = Get32BitUninstallRegistryKey();
            RegistryKey subKey = uninstallKey.CreateSubKey(RegistryKeyName);
            subKey.SetValue("DisplayName", uninstallDisplayName);
            subKey.SetValue("UninstallString", Path.Combine(getBinPath(), "BINDInstall.exe"));

            RegistryKey bindKey = getBindKey();
            bindKey.SetValue("InstallDir", getRootPath());
        }

        private RegistryKey getBindKey()
        {
            RegistryKey softwareKey = Get32BitSoftwareRegistryKey();

            RegistryKey iscKey;
            if (softwareKey.GetSubKeyNames().Contains("ISC"))
            {
                iscKey = softwareKey.OpenSubKey("ISC", true);
            }
            else
            {
                iscKey = softwareKey.CreateSubKey("ISC");
            }

            RegistryKey bindKey;
            if (iscKey.GetSubKeyNames().Contains("BIND"))
            {
                bindKey = iscKey.OpenSubKey("BIND", true);
            }
            else
            {
                bindKey = iscKey.CreateSubKey("BIND");
            }
            return bindKey;
        }

        private void unregisterApplication()
        {
            RegistryKey key = Get32BitUninstallRegistryKey();
            if (key.GetSubKeyNames().Contains(RegistryKeyName))
            {
                key.DeleteSubKey(RegistryKeyName);
            }
        }

        private void createServiceAccount()
        {
            windowsUser = new WindowsUser(
                accountUserName,
                accountPassword,
                accountDisplayName,
                accountDescription,
                WindowsUserFlag.PasswordCannotChange |
                WindowsUserFlag.PasswordNeverExpires);

            WindowsUserManager manager = new WindowsUserManager(Environment.MachineName);
            manager.Create(windowsUser);
            manager.GrantLogonAsService(windowsUser);
        }

        private bool removeServiceAccount()
        {
            bool result;
            try
            {
                WindowsUserManager manager = new WindowsUserManager(Environment.MachineName);
                if (manager.Exists(accountUserName))
                {
                    manager.Delete(accountUserName);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;

                DialogResult dr = MessageBox.Show(
                    "Could not remove ISC BIND service account.\r\n\r\n" + ex.Message,
                    "Remove service account",
                    MessageBoxButtons.AbortRetryIgnore, 
                    MessageBoxIcon.Warning);

                switch (dr)
                {
                    case DialogResult.Retry:
                        removeServiceAccount();
                        break;

                    case DialogResult.Ignore:
                        result = true;
                        break;
                }
            }
            return result;
        }

        private bool startService()
        {
            ServiceController sc = new ServiceController(serviceProcessName);
            return sc.Start(TimeSpan.FromSeconds(10));
        }

        private bool stopService()
        {
            bool result;
            try
            {
                ServiceController sc = new ServiceController(serviceProcessName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    // Only attempt to stop service if it's not already stopped.
                    result = sc.Stop(TimeSpan.FromSeconds(10));
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;

                DialogResult dr = MessageBox.Show(
                    "Could not stop ISC BIND service.\r\n\r\n" + ex.Message,
                    "Stop service", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);

                switch (dr)
                {
                    case DialogResult.Retry:
                        stopService();
                        break;

                    case DialogResult.Ignore:
                        result = true;
                        break;
                }
            }
            return result;
        }

        private void extractProgramFiles()
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(GetInstallPath(SetupWizard.SetupPath), getBinPath(), null);
        }

        private string getRootPath()
        {
            //return Path.Combine(Get32BitSystemPath(), rootDirectoryName);
            return Path.Combine(Get32BitProgramFilesPath(), rootDirectoryName);
        }

        private string getBinPath()
        {
            return Path.Combine(getRootPath(), binDirectoryName);
        }

        private string getEtcPath()
        {
            return Path.Combine(getRootPath(), etcDirectoryName);
        }

        private void deleteProgramFiles()
        {
            if (Directory.Exists(getBinPath()))
            {
                Directory.Delete(getBinPath(), true);
            }
        }

        private string createScArg(string name, string value)
        {
            return string.Format("{0}= \"{1}\"", name, value);
        }

        private bool createService(out int errorCode)
        {
            List<string> args = new List<string>();
            args.Add("create " + serviceProcessName);
            args.Add(createScArg("binPath", Path.Combine(getBinPath(), serviceProcessBin)));
            args.Add(createScArg("start", "auto"));
            args.Add(createScArg("DisplayName", serviceDisplayName));
            args.Add(createScArg("obj", @".\" + accountUserName));
            args.Add(createScArg("password", accountPassword));

            ProcessStartInfo startInfo = new ProcessStartInfo("sc", string.Join(" ", args.ToArray()));
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
            errorCode = p.ExitCode;

            if (errorCode != 0)
            {
                Registry.SetValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + serviceProcessName,
                    "Description",
                    serviceDescription);
            }

            return (p.ExitCode == 0);
        }

        private bool removeService()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("sc", "delete " + serviceProcessName);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
            bool result = (p.ExitCode == 0);

            if (!result)
            {
                DialogResult dr = MessageBox.Show(
                    "Could not uninstall ISC BIND service.\r\n\r\n" +
                    "SC exit code: " + p.ExitCode,
                    "Uninstall service",
                    MessageBoxButtons.AbortRetryIgnore,
                    MessageBoxIcon.Warning);

                switch (dr)
                {
                    case DialogResult.Retry:
                        removeService();
                        break;

                    case DialogResult.Ignore:
                        result = true;
                        break;
                }
            }

            return result;
        }
    }
}
