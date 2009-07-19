using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.Wizard;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.ManagerClient.DataViewing;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class WebsiteWizardPage : WizardPage
    {
        new public WebsiteWizardForm ParentWizard
        {
            get { return (WebsiteWizardForm)base.ParentWizard; }
        }

        public Website Website
        {
            get { return ParentWizard.Website; }
        }

        public WebsiteViewer Viewer
        {
            get { return ParentWizard.Viewer; }
        }

        public WebsiteWizardPage()
        {
            InitializeComponent();
        }
    }
}
