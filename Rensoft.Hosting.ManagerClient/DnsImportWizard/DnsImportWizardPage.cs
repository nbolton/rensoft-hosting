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
    public partial class DnsImportWizardPage : Rensoft.Windows.Forms.Wizard.WizardPage
    {
        new public DnsImportWizardForm ParentWizard
        {
            get { return (DnsImportWizardForm)base.ParentWizard; }
        }

        public DnsImportWizardPage()
        {
            InitializeComponent();
        }

        public RhspClientContext GetSourceContext()
        {
            return ((DnsImportWizardForm)ParentWizard).SourceContext;
        }

        public void SetSourceContext(RhspClientContext context)
        {
            ((DnsImportWizardForm)ParentWizard).SourceContext = context;
        }

        public IEnumerable<DnsZone> GetImportZones()
        {
            return ((DnsImportWizardForm)ParentWizard).ImportZones;
        }

        public void SetImportZones(IEnumerable<DnsZone> zones)
        {
            ((DnsImportWizardForm)ParentWizard).ImportZones = zones;
        }
    }
}
