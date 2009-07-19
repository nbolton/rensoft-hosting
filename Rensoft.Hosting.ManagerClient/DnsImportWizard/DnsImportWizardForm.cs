using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DnsImportWizard
{
    public partial class DnsImportWizardForm : Rensoft.Windows.Forms.Wizard.WizardForm
    {
        public RhspClientContext SourceContext { get; set; }
        public IEnumerable<DnsZone> ImportZones { get; set; }
        public DnsImportWizardMode ImportMode { get; set; }

        public DnsImportWizardForm()
        {
            InitializeComponent();
            AddPage(new SourcePage());
            AddPage(new SelectPage());
            AddPage(new FinishPage());
        }
    }
}
