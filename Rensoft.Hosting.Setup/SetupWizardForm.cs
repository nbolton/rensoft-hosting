using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.Wizard;
using Rensoft.Hosting.Setup.Options;
using System.IO;

namespace Rensoft.Hosting.Setup
{
    public partial class SetupWizardForm : WizardForm
    {
        //public ServerConfig ServerConfig { get; set; }
        //public bool InstallServer { get; set; }
        //public bool InstallManager { get; set; }
        //public SetupOption ServerOption { get; set; }
        //public SetupOption ManagerOption { get; set; }

        public string SetupPath { get; private set; }
        public List<SetupOption> OptionList { get; private set; }

        public SetupWizardForm()
        {
            OptionList = new List<SetupOption>();

            InitializeComponent();
            initializePages();
            initializeOptions();

            SetupPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void initializeOptions()
        {
            OptionList.Clear();
            OptionList.Add(new ServerSetupOption(this));
            OptionList.Add(new ManagerSetupOption(this));
            OptionList.Add(new FileZillaSetupOption(this));
            OptionList.Add(new IscBindSetupOption(this));
        }

        private void initializePages()
        {
            AddPage(new ComponentPage());
            AddPage(new SettingsPage1());
            AddPage(new SettingsPage2());
            AddPage(new InstallationPage());
        }
    }
}
