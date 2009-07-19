using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.Setup.Options;
using System.Linq;

namespace Rensoft.Hosting.Setup
{
    public partial class SettingsPage2 : Rensoft.Hosting.Setup.SetupWizardPage
    {
        public SettingsPage2()
        {
            InitializeComponent();

            dnsIpsTextBox.Text = string.Join(",", GetIps().ToArray());

            BeforeLoadAsync += new DoWorkEventHandler(SettingsPage2_BeforeLoadAsync);
            BeforeNextAsync += new DoWorkEventHandler(SettingsPage2_BeforeNextAsync);
        }

        void SettingsPage2_BeforeNextAsync(object sender, DoWorkEventArgs e)
        {
            if (ServerOptionExists())
            {
                if (string.IsNullOrEmpty(primaryTextBox.Text))
                {
                    MessageBox.Show(
                        "The primary SOA host for all zones must not be empty.",
                        "Primary SOA host",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(hostmasterTextBox.Text))
                {
                    MessageBox.Show(
                        "The SOA hostmaster for all zones must not be empty.",
                        "SOA hostmaster",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(dnsIpsTextBox.Text))
                {
                    MessageBox.Show(
                        "The list of suggested DNS A record IPs must not be empty.",
                        "Suggest DNS IPs",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(dnsServersTextBox.Text))
                {
                    MessageBox.Show(
                        "The list of suggested DNS servers must not be empty.",
                        "Suggest DNS servers",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                ServerSetupOption option = OptionList.OfType<ServerSetupOption>().Single();
                option.DnsSoaPrimaryServer = primaryTextBox.Text;
                option.DnsSoaHostmaster = hostmasterTextBox.Text;
                option.DnsSuggestAIPs = dnsIpsTextBox.Text;
                option.DnsSuggestServers = dnsServersTextBox.Text;
            }
        }

        void SettingsPage2_BeforeLoadAsync(object sender, DoWorkEventArgs e)
        {
            groupBox1.Enabled = ServerOptionExists();
        }
    }
}
