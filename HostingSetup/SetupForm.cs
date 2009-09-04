using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HostingSetup.Properties;
using System.Diagnostics;

namespace HostingSetup
{
    public partial class SetupForm : Form
    {
        private DirectoryInfo setupDirectory;
        private List<SetupFileInfo> infoList;

        public SetupForm()
        {
            InitializeComponent();
            Load += new EventHandler(SetupForm_Load);
        }

        private void initializeInfoList()
        {
            infoList = new List<SetupFileInfo>();

            // Files to support the Rensoft.Hosting.Setup application.
            add("ICSharpCode.SharpZipLib.dll", Resources.ICSharpCode_SharpZipLib_dll);
            add("Interop.WindowsInstaller.dll", Resources.Interop_WindowsInstaller_dll);
            add("Rensoft.dll", Resources.Rensoft_dll);
            add("Rensoft.Hosting.DataAccess.dll", Resources.Rensoft_Hosting_DataAccess_dll);
            add("Rensoft.Hosting.Setup.exe", Resources.Rensoft_Hosting_Setup);
            add("Rensoft.Hosting.Setup.exe.config", Resources.Rensoft_Hosting_Setup_exe_config);
            add("Rensoft.Windows.Forms.dll", Resources.Rensoft_Windows_Forms_dll);
            add("Rensoft.ServerManagement.dll", Resources.Rensoft_ServerManagement_dll);
            add("Rensoft.Hosting.dll", Resources.Rensoft_Hosting_dll);

            // Files to be launched within the Rensoft.Hosting.Setup application.
            add("HostingManagerSetup.msi", Resources.HostingManagerSetup);
            add("HostingServerSetup.msi", Resources.HostingServerSetup);
            add("BIND9.5.0-P2-W2.zip", Resources.BIND9_5_0_P2_W2);
            add("filezilla_server-0_9_28.exe", Resources.filezilla_server_0_9_28);
        }

        private void add(string targetFile, object resource)
        {
            infoList.Add(new SetupFileInfo(targetFile, resource));
        }

        void SetupForm_Load(object sender, EventArgs e)
        {
            startBackgroundWorker.RunWorkerAsync();
        }

        private void startBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            initializeInfoList();

            setupDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            setupDirectory.Create();

            foreach (SetupFileInfo info in infoList)
            {
                if (info.Resource != null)
                {
                    string targetPath = Path.Combine(setupDirectory.FullName, info.TargetFile);
                    if (info.Resource is byte[])
                    {
                        File.WriteAllBytes(targetPath, (byte[])info.Resource);
                    }
                    else if (info.Resource is string)
                    {
                        File.WriteAllText(targetPath, (string)info.Resource);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "File resource type '" + info.Resource.GetType() + "' not supported.");
                    }
                }
            }
        }

        private void startBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(
                    "An error occured while extracting files.\r\n\r\n" +
                    e.Error.Message,
                    "Setup error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Process process = Process.Start(
                        Path.Combine(setupDirectory.FullName, "Rensoft.Hosting.Setup.exe"));

                    Hide();

                    // Wait for setup exit, then clean up.
                    process.WaitForExit();
                    setupDirectory.Delete(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "An error occured.\r\n\r\n" +
                        ex.Message,
                        "Setup error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            // Now close once cleaned up.
            Close();
        }
    }
}
