using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DnsImportWizard
{
    public partial class FinishPage : DnsImportWizardPage
    {
        public FinishPage()
        {
            InitializeComponent();

            BeforeNextAsync += new DoWorkEventHandler(FinishPage_BeforeNextAsync);
            NextAsync += new DoWorkEventHandler(FinishPage_NextAsync);
        }

        void FinishPage_BeforeNextAsync(object sender, DoWorkEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Importing these DNS zones will replace any " +
                "existing zones which share the same name.",
                "Replace warning",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                createPanel.Visible = true;
            }
        }

        void FinishPage_NextAsync(object sender, DoWorkEventArgs e)
        {
            tryImport();
        }

        private void tryImport()
        {
            try
            {
                DnsZoneAdapter dzAdapter = LocalContext.Default.CreateAdapter<DnsZoneAdapter>();
                dzAdapter.ReplaceBatch(GetImportZones().Cast<object>().ToArray());
            }
            catch (Exception ex)
            {
                DialogResult result = MessageBox.Show(
                    "An error occured while creating the DNS zones.\r\n\r\n" +
                    "Error: " + ex.Message,
                    "Import error",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Error);

                if (result == DialogResult.Retry)
                {
                    tryImport();
                }
            }
        }
    }
}
