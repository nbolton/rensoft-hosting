using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rensoft.Hosting.ManagerClient.DnsImportWizard
{
    public partial class SourcePage : DnsImportWizardPage
    {
        public SourcePage()
        {
            InitializeComponent();

            EnableNextAfterLoad = false;
            AfterLoadAsync += new RunWorkerCompletedEventHandler(SourcePage_AfterLoadAsync);
        }

        void SourcePage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            // User may have clicked back to get to this page.
            EnableNext = (GetSourceContext() != null);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ConnectionDialog cd = new ConnectionDialog();
            DialogResult result = cd.ShowDialog();

            if (result == DialogResult.OK)
            {
                SetSourceContext(cd.CreateClientContext());
                statusLabel.Text = "Connected to " + getSourceAddress() + ".";
                hintLabel.Visible = false;
                EnableNext = true;
            }
        }

        private string getSourceAddress()
        {
            return GetSourceContext().Address.Uri.GetHostAndPort();
        }
    }
}
