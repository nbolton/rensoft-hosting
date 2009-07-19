using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public partial class FtpUserCreator : Form
    {
        public FtpUserCreator()
        {
            InitializeComponent();
        }

        public FtpUser GetFtpUser()
        {
            FtpUser user = new FtpUser();
            user.UserName = userNameTextBox.Text;
            user.Password = LocalContext.Default.EncryptPassword(passwordTextBox1.Text);
            user.Enabled = true;
            user.PendingAction = ChildPendingAction.Create;
            return user;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox1.Text))
            {
                MessageBox.Show(
                    "A user name must be specified.",
                    "User name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox1.Text))
            {
                MessageBox.Show(
                    "A password must be specified.",
                    "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (passwordTextBox1.Text != passwordTextBox2.Text)
            {
                MessageBox.Show(
                    "The confirmation password does not match the original.",
                    "Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
