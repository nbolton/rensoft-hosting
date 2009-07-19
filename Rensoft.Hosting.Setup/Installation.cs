using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Installer.Client.Properties;
using System.Threading;
using Rensoft.Installer.InstallerWebService;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Rensoft.Installer.Client
{
    public partial class Installation : Rensoft.Windows.Forms.Wizard.WizardPage
    {
        private bool failuresOccured;

        protected delegate int AddStatusInvoker(string component);

        public Installation()
        {
            InitializeComponent();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
            {
                IsBusy = true;
                enableBack = false;
                enableNext = false;

                installDataGridView.Rows.Clear();
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void updateStatus(int rowIndex, InstallStatus status)
        {
            if (status == InstallStatus.Failed)
            {
                throw new NotSupportedException("Failed status expects error parameter.");
            }
            updateStatus(rowIndex, status, null);
        }

        private void updateStatus(int rowIndex, InstallStatus status, string error)
        {
            switch (status)
            {
                case InstallStatus.InProgress:
                    installDataGridView.Rows[rowIndex].Cells[0].Value = Resources.InProgress;
                    installDataGridView.Rows[rowIndex].Cells[2].Value = "In Progress";
                    installDataGridView.Rows[rowIndex].Cells[3].Value = null;
                    break;

                case InstallStatus.Success:
                    installDataGridView.Rows[rowIndex].Cells[0].Value = Resources.Success;
                    installDataGridView.Rows[rowIndex].Cells[2].Value = "Success";
                    installDataGridView.Rows[rowIndex].Cells[3].Value = null;
                    break;

                case InstallStatus.Failed:
                    installDataGridView.Rows[rowIndex].Cells[0].Value = Resources.Failure;
                    installDataGridView.Rows[rowIndex].Cells[2].Value = "Failed";
                    installDataGridView.Rows[rowIndex].Cells[3].Value = error;
                    failuresOccured = true;
                    break;
            }
        }

        private int invokeAddStatus(string component)
        {
            AddStatusInvoker method = new AddStatusInvoker(addStatus);
            return (int)Invoke(method, component);
        }

        private int addStatus(string component)
        {
            return installDataGridView.Rows.Add(Resources.Pending, component, "Pending");
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Add status rows and map IDs so they are easily accessable.
            Dictionary<string, int> rowMap = new Dictionary<string, int>();
            foreach (ApplicationSetup setup in MainScreen.ApplicationSetups)
            {
                rowMap[setup.Name + "_Download"] = invokeAddStatus("Downloading " + setup.Title);
                rowMap[setup.Name + "_Install"] = invokeAddStatus("Installing " + setup.Title);
            }

            // On 2nd iteration, action each component.
            foreach (ApplicationSetup setup in MainScreen.ApplicationSetups)
            {
                string setupFileName = Path.GetTempFileName() + ".msi";

                WebClient client = new WebClient();
                client.Proxy = WebRequest.GetSystemWebProxy();
                client.Proxy.Credentials = CredentialCache.DefaultCredentials;

                try
                {
                    updateStatus(rowMap[setup.Name + "_Download"], InstallStatus.InProgress);

                    // Download then update status.
                    client.DownloadFile(setup.SetupUrl, setupFileName);

                    updateStatus(rowMap[setup.Name + "_Download"], InstallStatus.Success);
                }
                catch (Exception ex)
                {
                    updateStatus(rowMap[setup.Name + "_Download"],
                        InstallStatus.Failed,
                        ex.Message);

                    updateStatus(rowMap[setup.Name + "_Install"],
                        InstallStatus.Failed,
                        "Download failed.");

                    continue;
                }


                updateStatus(rowMap[setup.Name + "_Install"], InstallStatus.InProgress);

                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(setupFileName, "/passive");
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    updateStatus(rowMap[setup.Name + "_Install"], InstallStatus.Success);
                }
                else
                {
                    updateStatus(
                        rowMap[setup.Name + "_Install"],
                        InstallStatus.Failed,
                        process.ExitCode.ToString());

                    continue;
                }

                if (setup.AutoStart)
                {
                    // Replace MSI style program files identifier.
                    string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    string installPath = setup.InstallPath.Replace("[ProgramFilesFolder]", pf);

                    // Assume path is correct and name is the name of the file.
                    string applicationPath = installPath + setup.Name;

                    // Pass installation key as upgrade key to automatically get settings.
                    Process.Start(applicationPath, "/UpgradeKey=" + MainScreen.InstallationKey);
                }
            }

            if (!failuresOccured)
            {
                // Automatically exit if no failures.
                Application.Exit();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsBusy = false;
            installDataGridView.Cursor = Cursors.Default;
            enableNext = !failuresOccured;
            enableBack = true;
        }

        enum InstallStatus
        {
            Pending,
            InProgress,
            Success,
            Failed
        }
    }
}

