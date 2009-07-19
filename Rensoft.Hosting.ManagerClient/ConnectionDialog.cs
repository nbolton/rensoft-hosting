using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.ManagerClient.Properties;
using System.Net;
using Rensoft.Hosting.DataAccess;
using System.Security.Principal;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.ManagerClient
{
    public partial class ConnectionDialog : Form
    {
        private string encryptorSecret;

        public string ServiceUri { get; protected set; }
        public RhspAuthMode AuthMode { get; protected set; }
        public NetworkCredential CustomCredentials { get; protected set; }

        public ConnectionDialog()
        {
            InitializeComponent();
            initializeAuthComboBox();
            initializeDefaultValues();
        }

        private void initializeDefaultValues()
        {
            if (ConfigManager.Default.ServerNameArray.Length != 0)
            {
                serverNameComboBox.DataSource = ConfigManager.Default.ServerNameArray;
            }
            else
            {
                serverNameComboBox.DataSource = new string[] { 
                    Settings.Default.DefaultServiceUri
                };
            }

            selectAuthComboBoxValue(ConfigManager.Default.AuthMode);
        }

        private void selectAuthComboBoxValue(RhspAuthMode mode)
        {
            foreach (AuthModeItem item in authComboBox.Items)
            {
                if (item.Mode == mode)
                {
                    authComboBox.SelectedItem = item;
                }
            }
        }

        private void initializeAuthComboBox()
        {
            AuthModeItem[] authItems = new AuthModeItem[] 
            {
                new AuthModeItem() { Mode = RhspAuthMode.Default, Title = "Current Windows User" },
                new AuthModeItem() { Mode = RhspAuthMode.Custom, Title = "Other Windows User" }
            };

            authComboBox.DisplayMember = "Title";
            authComboBox.DataSource = authItems;
        }

        private void dividerPictureBox_Paint(object sender, PaintEventArgs e)
        {
            int width = dividerPictureBox.Width;
            Pen pen1 = new Pen(Color.FromArgb(160, 160, 160));
            Pen pen2 = new Pen(Color.FromArgb(240, 240, 240));
            e.Graphics.DrawLine(pen1, new Point(0, 0), new Point(width, 0));
            e.Graphics.DrawLine(pen2, new Point(0, 1), new Point(width, 1));
        }

        public class AuthModeItem
        {
            public RhspAuthMode Mode { get; set; }
            public string Title { get; set; }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            reflectControlValues();
            runTestAsync();
        }

        private void reflectControlValues()
        {
            updatePropertyValues();
            updateConfigValues();
        }

        private void updatePropertyValues()
        {
            ServiceUri = serverNameComboBox.Text;
            AuthMode = getSelectedAuthMode();

            CustomCredentials = new NetworkCredential(
                userNameTextBox.Text,
                passwordTextBox.Text);
        }

        private void updateConfigValues()
        {
            ConfigManager.Default.AuthMode = getSelectedAuthMode();
            ConfigManager.Default.ServerNameArray = getServerNameArray();
        }

        private string[] getServerNameArray()
        {
            List<string> stringList = new List<string>();

            // Get the value typed by the user.
            stringList.Add(serverNameComboBox.Text);

            // Get all the original values.
            foreach (object value in serverNameComboBox.Items)
            {
                string stringValue = (string)value;

                // Ensure items aren't duplicated.
                if (stringValue != serverNameComboBox.Text)
                {
                    stringList.Add(stringValue);
                }
            }

            return stringList.ToArray();
        }

        private RhspAuthMode getSelectedAuthMode()
        {
            return ((AuthModeItem)authComboBox.SelectedItem).Mode;
        }

        private void authComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (getSelectedAuthMode() == RhspAuthMode.Custom)
            {
                userNameTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                userNameTextBox.Text = string.Empty;
            }
            else
            {
                userNameTextBox.Enabled = false;
                passwordTextBox.Enabled = false;
                userNameTextBox.Text = WindowsIdentity.GetCurrent().Name;
            }
        }

        public RhspClientContext CreateClientContext()
        {
            reflectControlValues();

            RhspClientContext context = new RhspClientContext(
                ServiceUri,
                new RhspSecurity
                {
                    AuthMode = AuthMode,
                    CustomCredentials = CustomCredentials
                }
            )
            {
                EncryptorSecret = encryptorSecret
            };

#if DEBUG
            // Stop client from disconnecting during debug.
            context.Timeout = TimeSpan.FromHours(1);
#endif

            return context;
        }

        private void runTestAsync()
        {
            Cursor = Cursors.WaitCursor;
            connectButton.Enabled = false;

            if (!testBackgroundWorker.IsBusy)
            {
                TestArgs args = new TestArgs() { Context = CreateClientContext() };
                testBackgroundWorker.RunWorkerAsync(args);
            }
        }

        private void testBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TestArgs args = (TestArgs)e.Argument;
            DiagnosticAdapter dAdapter = args.Context.CreateAdapter<DiagnosticAdapter>();
            ServerConfigAdapter scAdapter = args.Context.CreateAdapter<ServerConfigAdapter>();

            ConnectionTestResult result = dAdapter.TestConnection();

            if (result.Success)
            {
                // Set to class var for collection in CreateClientContext.
                encryptorSecret = scAdapter.EncryptorSecret;
            }

            e.Result = new TestResult()
            {
                Success = result.Success,
                Message = result.Message
            };
        }

        private void testBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Default;
            connectButton.Enabled = true;

            if (e.Error != null)
            {
                throw new Exception("Unable to test connection.", e.Error);
            }

            TestResult result = (TestResult)e.Result;
            if (result.Success)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Unable to establish connection to server.\r\n" +
                    "Message: " + result.Message,
                    "Connection failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected class TestArgs
        {
            public RhspClientContext Context { get; set; }
        }

        protected class TestResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ConnectionDialog_Shown(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
