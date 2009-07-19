using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataEditing;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    public partial class CustomerFtpUsersEditorControl : Rensoft.Hosting.ManagerClient.DataEditing.LocalDataEditorControl
    {
        private List<FtpUser> resultList;
        private BindingList<FtpUser> bindingList;

        public CustomerFtpUsersEditorControl()
        {
            InitializeComponent();

            this.resultList = new List<FtpUser>();
            this.bindingList = new BindingList<FtpUser>();

            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = bindingList;

            ReflectDataToForm += new DataEditorReflectEventHandler(CustomerFtpUsersEditorControl_ReflectDataToForm);
            ReflectFormToData += new DataEditorReflectEventHandler(CustomerFtpUsersEditorControl_ReflectFormToData);
            ValidateData += new CancelEventHandler(CustomerFtpUsersEditorControl_ValidateData);
        }

        void CustomerFtpUsersEditorControl_ValidateData(object sender, CancelEventArgs e)
        {
            if (enableCheckBox.Checked && (bindingList.Count == 0))
            {
                e.Cancel = true;
                MessageBox.Show(
                    "FTP access cannot be enabled without adding FTP users.",
                    "FTP access", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void CustomerFtpUsersEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            resultList.Clear();
            if (e.GetData<Customer>().FtpUserArray != null)
            {
                resultList.AddRange(e.GetData<Customer>().FtpUserArray);
            }

            bindingList.Clear();
            if (e.GetData<Customer>().FtpEnabled)
            {
                enableCheckBox.Checked = true;
                resultList.ForEach(ftpUser => bindingList.Add(ftpUser));
            }
            bindingList.ResetBindings();
        }

        void CustomerFtpUsersEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            if (enableCheckBox.Checked)
            {
                dataGridView.EndEdit();
                e.GetData<Customer>().FtpUserArray = resultList.ToArray();
                e.GetData<Customer>().FtpEnabled = true;
            }
            else
            {
                e.GetData<Customer>().FtpEnabled = false;
            }
        }

        private void enableCheckBox_BeforeCheckedChanged(object sender, CancelEventArgs e)
        {
            // If nothing in binding list, don't bother asking.
            if (enableCheckBox.Checked && (bindingList.Count != 0))
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to disable FTP access?\r\n" +
                    "This will delete all FTP users for this customer.",
                    "Disable FTP access",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    resultList.ForEach(ftpUser => deleteFtpUser(ftpUser));
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView.Enabled = enableCheckBox.Checked;
            addButton.Enabled = enableCheckBox.Checked;
            removeButton.Enabled = enableCheckBox.Checked;ChangeMade();
            ChangeMade();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            FtpUserCreator creator = new FtpUserCreator();
            DialogResult result = creator.ShowDialog();

            if (result == DialogResult.OK)
            {
                FtpUser ftpUser = creator.GetFtpUser();
                resultList.Add(ftpUser);
                bindingList.Add(ftpUser);
                bindingList.ResetBindings();
                ChangeMade();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 0)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete the selected FTP users?",
                    "Delete FTP users", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    List<DataGridViewRow> list = dataGridView.SelectedRows.Cast<DataGridViewRow>().ToList();
                    list.ForEach(r => deleteFtpUser((FtpUser)r.DataBoundItem));
                    bindingList.ResetBindings();
                    ChangeMade();
                }
            }
        }

        private void deleteFtpUser(FtpUser ftpUser)
        {
            // Mark deleted so that it is deleted on server request.
            ftpUser.PendingAction = ChildPendingAction.Delete;

            // Remove from visual binding list.
            bindingList.Remove(ftpUser);
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                FtpUser ftpUser = bindingList[e.RowIndex];
                if (ftpUser.PendingAction != ChildPendingAction.Create)
                {
                    ftpUser.PendingAction = ChildPendingAction.Update;
                }
            }
        }
    }
}
