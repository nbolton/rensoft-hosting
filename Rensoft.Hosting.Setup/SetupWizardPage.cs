using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.Setup.Options;
using System.Net;
using System.Linq;
using System.Net.Sockets;

namespace Rensoft.Hosting.Setup
{
    public partial class SetupWizardPage : Rensoft.Windows.Forms.Wizard.WizardPage
    {
        new public SetupWizardForm ParentWizard
        {
            get { return (SetupWizardForm)base.ParentWizard; }
        }

        public List<SetupOption> OptionList
        {
            get { return ParentWizard.OptionList; }
        }

        public SetupWizardPage()
        {
            InitializeComponent();
        }

        protected IEnumerable<string> GetIps()
        {
            IPHostEntry entry = Dns.GetHostEntry(Environment.MachineName);
            return entry.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Select(ip => ip.ToString());
        }

        protected bool ServerOptionExists()
        {
            return (OptionList.OfType<ServerSetupOption>().Count(o => o.Enabled) != 0);
        }
    }
}
