using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class FinishPage : Rensoft.Hosting.ManagerClient.WebsiteWizard.WebsiteWizardPage
    {
        public FinishPage()
        {
            InitializeComponent();

            BeforeNextAsync += new DoWorkEventHandler(FinishPage_BeforeNextAsync);
            NextAsync += new DoWorkEventHandler(FinishPage_NextAsync);
            AfterNextAsync += new RunWorkerCompletedEventHandler(FinishPage_AfterNextAsync);
        }

        void FinishPage_AfterNextAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            if (showEditorCheckBox.Checked)
            {
                if (Viewer == null)
                {
                    throw new Exception("The Viewer property has not been set.");
                }

                WebsiteEditorForm editor = Viewer.GetEditor<WebsiteEditorForm>(Website);
                editor.SetUpdateMode(Website);
                Viewer.ShowEditorAsync(editor);
            }
        }

        void FinishPage_BeforeNextAsync(object sender, DoWorkEventArgs e)
        {
            createPanel.Visible = true;
        }

        void FinishPage_NextAsync(object sender, DoWorkEventArgs e)
        {
            WebsiteAdapter wa = LocalContext.Default.CreateAdapter<WebsiteAdapter>();
            wa.Create(Website);
        }
    }
}
