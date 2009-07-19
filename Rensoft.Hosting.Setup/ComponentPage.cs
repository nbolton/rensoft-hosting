using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsInstaller;
using System.Reflection;
using System.IO;
using Rensoft.Windows.Forms.Wizard;
using Rensoft.Hosting.Setup.Options;
using System.Linq;

namespace Rensoft.Hosting.Setup
{
    public partial class ComponentPage : SetupWizardPage
    {
        //private const string serverProductName = "Rensoft Hosting Server 2008";
        //private const string managerProductName = "Rensoft Hosting Manager 2008";
        //private const string serverFileName = "HostingServerSetup.msi";
        //private const string managerFileName = "HostingManagerSetup.msi";

        //private string serverVersion;
        //private string managerVersion;
        //private string serverFilePath;
        //private string managerFilePath;

        public ComponentPage()
        {
            InitializeComponent();

            //serverFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, serverFileName);
            //managerFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, managerFileName);

            //serverCheckBox.Text = serverProductName;
            //managerCheckBox.Text = managerProductName;
        }

        //private string getMsiVersion(string installerFileName)
        //{
        //    Record record = getMsiRecord(
        //        installerFileName,
        //        "SELECT * FROM Property WHERE Property = 'ProductVersion'");

        //    return record.get_StringData(2);
        //}

        //private Record getMsiRecord(string installerFileName, string query)
        //{
        //    Installer installer = (Installer)Activator.CreateInstance(
        //        Type.GetTypeFromProgID("WindowsInstaller.Installer"));

        //    Database database = installer.OpenDatabase(installerFileName, 0);
        //    WindowsInstaller.View view = database.OpenView(query);
        //    view.Execute(null);
        //    return view.Fetch();
        //}

        private void ComponentPage_LoadAsync(object sender, DoWorkEventArgs e)
        {
            //if (File.Exists(serverFilePath))
            //{
            //    serverVersion = getMsiVersion(serverFilePath);
            //}

            //if (File.Exists(managerFilePath))
            //{
            //    managerVersion = getMsiVersion(managerFilePath);
            //}
            OptionList.ForEach(o => o.Load());
        }

        private void ComponentPage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            //updateCheckBox(serverCheckBox, OptionList.OfType<ServerSetupOption>().First());
            //updateCheckBox(managerCheckBox, OptionList.OfType<ManagerSetupOption>().First());
            
            foreach (SetupOption option in OptionList)
            {
                flowLayoutPanel.Controls.Add(option.ComponentCheckBox);
            }

            refreshEnableNext();
        }

        //private void updateCheckBox(CheckBox checkBox, SetupOption option)
        //{
        //    if (File.Exists(option.MsiFileName))
        //    {
        //        checkBox.Text = option.ProductName;
        //    }
        //    else
        //    {
        //        //MessageBox.Show(
        //        //    "The setup file '" + 
        //    }
        //}

        private void refreshEnableNext()
        {
            //EnableNext = (serverCheckBox.Checked || managerCheckBox.Checked);
            EnableNext = OptionList.Any(o => o.Enabled);
        }

        private void serverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            refreshEnableNext();
        }

        private void managerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            refreshEnableNext();
        }

        private void ComponentPage_AfterNextAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            //((SetupWizardForm)ParentWizard).InstallServer = serverCheckBox.Checked;
            //((SetupWizardForm)ParentWizard).InstallManager = managerCheckBox.Checked;

            //if (serverCheckBox.Checked)
            //{
            //    OptionList.OfType<ServerSetupOption>().First().Enabled = true;

            //    //((SetupWizardForm)ParentWizard).ServerOption = new ServerOption()
            //    //{
            //    //    ProductName = serverProductName,
            //    //    MsiFilePath = serverFilePath,
            //    //    Status = SetupStatus.Pending
            //    //};
            //}

            //if (managerCheckBox.Checked)
            //{
            //    OptionList.OfType<ManagerSetupOption>().First().Enabled = true;

            //    //((SetupWizardForm)ParentWizard).ManagerOption = new ManagerOption()
            //    //{
            //    //    ProductName = managerProductName,
            //    //    MsiFilePath = managerFilePath,
            //    //    Status = SetupStatus.Pending
            //    //};
            //}
        }
    }
}
