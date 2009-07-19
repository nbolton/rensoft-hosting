using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Rensoft.Hosting.Setup.Options
{
    public class FileZillaSetupOption : SetupOption
    {
        private bool autoEnabled;

        public override string ProductName
        {
            get { return "FileZilla FTP Server 0.9.28"; }
        }

        public override string SetupFileName
        {
            get { return "filezilla_server-0_9_28.exe"; }
        }

        public FileZillaSetupOption(SetupWizardForm setupWizard)
            : base(setupWizard) { }

        public override void Load()
        {
            if (!autoEnabled)
            {
                // Do not suggest installation when already installed.
                Enabled = !IsInstalled();
                autoEnabled = true;
            }
        }

        //private const string registryKeyName = "FileZilla Server";

        public override string RegistryKeyName
        {
            get { return "FileZilla Server"; }
        }

        //private bool isInstalled()
        //{
        //    return GetUninstallRegistryKey().GetSubKeyNames().Count(n => n == registryKeyName) != 0;
        //}

        public override bool ProcessOption(UpdateStatusDelegate updateStatus)
        {
            if (!base.ProcessOption(updateStatus))
            {
                return false;
            }

            if (IsInstalled())
            {
                updateStatus(this, SetupStatus.InProgress, "Uninstalling...");
                Process u1 = Process.Start(GetUninstallPath(false));
                u1.WaitForExit();

                // Allow 2nd process to start.
                System.Threading.Thread.Sleep(1000);

                Process u2 = getUninstallQuery().First();
                u2.WaitForExit();

                if (IsInstalled())
                {
                    updateStatus(this, SetupStatus.Failure, "Uninstallation failed");
                    return false;
                }
            }

            updateStatus(this, SetupStatus.InProgress, "Installing...");
            Process.Start(GetInstallPath(SetupWizard.SetupPath));
            Process install = getInstallQuery().First();
            install.WaitForExit();

            // Allow 2nd process to start.
            System.Threading.Thread.Sleep(1000);

            if (!IsInstalled())
            {
                updateStatus(this, SetupStatus.Failure, "Installation failed");
                return false;
            }

            return true;
        }

        private IEnumerable<Process> getInstallQuery()
        {
            var q = from install in Process.GetProcesses()
                    where SetupFileName.StartsWith(install.ProcessName)
                    select install;
            return q;
        }

        private static IEnumerable<Process> getUninstallQuery()
        {
            var q = from uninstall in Process.GetProcesses()
                    where uninstall.MainWindowTitle.StartsWith("FileZilla Server")
                    where uninstall.MainWindowTitle.TrimEnd().EndsWith("Uninstall")
                    select uninstall;
            return q;
        }
    }
}
