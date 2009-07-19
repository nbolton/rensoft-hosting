using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Rensoft.Hosting.Setup.Options;
using System.Net;
using System.Net.Sockets;

namespace Rensoft.Hosting.Setup
{
    public partial class SettingsPage1 : SetupWizardPage
    {
        public SettingsPage1()
        {
            InitializeComponent();

            legacyConnectionStringTextBox.Text =
                "Server=" + Environment.MachineName + @"\SQLEXPRESS;Database=" +
                "RensoftHosting;Trusted_Connection=True";

            wsnTextBox.Text = Environment.MachineName;

            iisIpsTextBox.Text = string.Join(",", GetIps().ToArray()) + ",*";

            BeforeNextAsync += new DoWorkEventHandler(ConfigurePage_BeforeNextAsync);
            BeforeLoadAsync += new DoWorkEventHandler(ConfigurePage_BeforeLoadAsync);
        }

        void ConfigurePage_BeforeLoadAsync(object sender, DoWorkEventArgs e)
        {
            groupBox1.Enabled = ServerOptionExists();
            groupBox2.Enabled = ServerOptionExists();
        }

        void ConfigurePage_BeforeNextAsync(object sender, DoWorkEventArgs e)
        {
            if (ServerOptionExists())
            {
                if (legacyCheckBox.Checked
                    && string.IsNullOrEmpty(legacyConnectionStringTextBox.Text))
                {
                    MessageBox.Show(
                        "The legacy connection string was empty while legacy mode was enabled.",
                        "Legacy connection string",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(wsnTextBox.Text))
                {
                    MessageBox.Show(
                        "The windows server name text box cannot be empty.",
                        "Windows server name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (wsnTextBox.Text == "localhost")
                {
                    MessageBox.Show(
                        "Microsoft ADSI does not support windows server name 'localhost'.",
                        "Windows server name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (string.IsNullOrEmpty(iisIpsTextBox.Text))
                {
                    MessageBox.Show(
                        "The list of available IIS IPs must not be empty.",
                        "Available IIS IPs",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                if (encryptorSecretChangeCheckBox.Checked &&
                    string.IsNullOrEmpty(encryptorSecretTextBox.Text))
                {
                    MessageBox.Show(
                        "The encryption secret must not be empty when enabled.",
                        "Encryption secret",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }

                ServerSetupOption option = OptionList.OfType<ServerSetupOption>().Single();
                option.EnableLegacyMode = legacyCheckBox.Checked;
                option.LegacyConnectionString = legacyConnectionStringTextBox.Text;
                option.WindowsServerName = wsnTextBox.Text;
                option.AvailableIisIPs = iisIpsTextBox.Text;
                option.EncryptionSecret = encryptorSecretTextBox.Text;
                option.WebsiteDirectory = websiteDirectoryTextBox.Text;
                option.EncryptorSecretChange = encryptorSecretChangeCheckBox.Checked;
            }
        }

        private void legacyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            legacyConnectionStringTextBox.Enabled = legacyCheckBox.Checked;
        }

        private void websiteDirectoryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = websiteDirectoryFolderBrowserDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                websiteDirectoryTextBox.Text = websiteDirectoryFolderBrowserDialog.SelectedPath;
            }
        }

        private void encryptorSecretChangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            encryptorSecretTextBox.Enabled = encryptorSecretChangeCheckBox.Checked;
            if (encryptorSecretChangeCheckBox.Checked)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to set a new encryption secret? " +
                    "If this is the first installation, this is safe to do, " +
                    "but if you are installing over an existing installation, " +
                    "then setting a new secret may result in lost passwords.",
                    "Encryption secret",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    // Set the check box and text box to disabled.
                    encryptorSecretChangeCheckBox.Checked = !encryptorSecretChangeCheckBox.Checked;
                    encryptorSecretTextBox.Enabled = false;
                }
            }
        }

        private void encryptorSecretTextBox_EnabledChanged(object sender, EventArgs e)
        {
            if (encryptorSecretTextBox.Enabled)
            {
                encryptorSecretTextBox.Text = Guid.NewGuid().ToString().Replace("-", null);
            }
            else
            {
                encryptorSecretTextBox.Text = null;
            }
        }
    }
}
