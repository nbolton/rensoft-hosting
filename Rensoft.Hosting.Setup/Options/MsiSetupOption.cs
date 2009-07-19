using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using WindowsInstaller;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Rensoft.Hosting.Setup.Options
{
    public abstract class MsiSetupOption : SetupOption
    {
        public abstract string MsiTitle { get; }

        public override string RegistryKeyName
        {
            get { return getProductCode(); }
        }

        //private string getProductCode()
        //{
        //    RegistryKey baseKey = GetUninstallRegistryKey();

        //    var q = from k in baseKey.GetSubKeyNames().Select(n => baseKey.OpenSubKey(n))
        //            where k.GetValueNames().Contains("UninstallString")
        //            let uninstallPath = parseRegistryString(k.GetValue("UninstallString"))
        //            where uninstallPath.Contains(SetupFileName)
        //            select uninstallPath;

        //    return q.FirstOrDefault();
        //}

        private string getProductCode()
        {
            Record record = getMsiRecord(
                "SELECT * FROM Property WHERE Property = 'ProductCode'");

            return record.get_StringData(2);
        }

        private Record getMsiRecord(string query)
        {
            Installer installer = (Installer)Activator.CreateInstance(
                Type.GetTypeFromProgID("WindowsInstaller.Installer"));

            Database database = installer.OpenDatabase(SetupFilePath, 0);
            WindowsInstaller.View view = database.OpenView(query);
            view.Execute(null);
            return view.Fetch();
        }

        private string parseRegistryString(object registryString)
        {
            // Returns the string minus quotes at start and end.
            string value = (string)registryString;
            return value.Substring(1, value.Length - 2);
        }

        public MsiSetupOption(SetupWizardForm setupWizard)
            : base(setupWizard)
        {
            ComponentCheckBox.Checked = true;
        }

        public override bool ProcessOption(UpdateStatusDelegate updateStatus)
        {
            if (!base.ProcessOption(updateStatus))
            {
                return false;
            }

            if (IsInstalled())
            {
                Process uninstallProcess = GetUninstallProcess();

                // Uninstall in case application is already installed.
                updateStatus(this, SetupStatus.InProgress, "Uninstalling...");
                uninstallProcess.Start();
                uninstallProcess.WaitForExit();

                if (uninstallProcess.ExitCode != 0)
                {
                    updateStatusUninstallFailed(uninstallProcess, updateStatus);
                    return false;
                }
            }

            Process installProcess = GetInstallProcess(SetupWizard.SetupPath);

            // Now run the installation quietly with no interface.
            updateStatus(this, SetupStatus.InProgress, "Installing...");
            installProcess.Start();
            installProcess.WaitForExit();

            if (installProcess.ExitCode != 0)
            {
                updateStatusUninstallFailed(installProcess, updateStatus);
                return false;
            }

            return true;
        }

        private void updateStatusInstallFailed(Process installProcess, UpdateStatusDelegate updateStatus)
        {
            updateStatus(this, SetupStatus.Failure, "Install failed: " + installProcess.ExitCode.ToString());
        }

        private void updateStatusUninstallFailed(Process uninstallProcess, UpdateStatusDelegate updateStatus)
        {
            updateStatus(this, SetupStatus.Failure, "Uninstall failed:" + uninstallProcess.ExitCode.ToString());
        }

        public Process GetInstallProcess(string setupPath)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("msiexec", "/package \"" + GetFullMsiPath(setupPath) + "\" /qn");
            return p;
        }

        public string GetFullMsiPath(string setupPath)
        {
            return Path.Combine(setupPath, SetupFileName);
        }

        public Process GetUninstallProcess()
        {
            //TODO: uninstall based on installed msi guid
            Process p = new Process();
            //p.StartInfo = new ProcessStartInfo("msiexec", "/uninstall \"" + GetFullMsiPath(setupPath) + "\" /qn");
            p.StartInfo = new ProcessStartInfo("msiexec", "/uninstall " + getProductCode() + " /qn");
            p.StartInfo.UseShellExecute = false;
            return p;
        }
    }
}
