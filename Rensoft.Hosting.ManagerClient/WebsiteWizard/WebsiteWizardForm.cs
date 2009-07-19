using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using Rensoft.Hosting.ManagerClient.DataViewing;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class WebsiteWizardForm : Rensoft.Windows.Forms.Wizard.WizardForm
    {
        public Website Website { get; private set; }
        public WebsiteViewer Viewer { get; private set; }

        public WebsiteWizardForm()
            : this(null) { }

        public WebsiteWizardForm(WebsiteViewer viewer)
        {
            InitializeComponent();

            this.Viewer = viewer;
            this.Website = new Website(RhspDataID.Generate());

            AddPage(new WelcomePage());
            AddPage(new CustomerPage());
            AddPage(new SettingsPage());
            AddPage(new FinishPage());
        }
    }
}
