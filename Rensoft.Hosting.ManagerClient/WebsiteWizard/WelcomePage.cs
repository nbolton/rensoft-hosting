using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class WelcomePage : Rensoft.Hosting.ManagerClient.WebsiteWizard.WebsiteWizardPage
    {
        public WelcomePage()
        {
            InitializeComponent();

            AfterNextAsync += new RunWorkerCompletedEventHandler(WelcomePage_AfterNextAsync);
        }

        void WelcomePage_AfterNextAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            if (manualRadioButton.Checked)
            {
                WebsiteEditorForm editor = Viewer.GetEditor<WebsiteEditorForm>(Website);
                editor.SetCreateMode(Website);
                Viewer.ShowEditorAsync(editor);
            }
        }

        private void manualRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            IsLastPage = manualRadioButton.Checked;
        }
    }
}
