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
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    public partial class WebsiteSecurityEditorControl : Rensoft.Hosting.ManagerClient.DataEditing.LocalDataEditorControl
    {
        private List<SecurityTemplate> ruleResultList;
        private BindingList<SecurityTemplate> ruleBindingList;
        //private BindingList<String> usernameBindingList;

        //public IEnumerable<string> Usernames
        //{
        //    get { return usernameBindingList; }
        //    set
        //    {
        //        usernameBindingList.Clear();
        //        value.ToList().ForEach(u => usernameBindingList.Add(u));
        //        usernameBindingList.ResetBindings();
        //    }
        //}

        public WebsiteSecurityEditorControl()
        {
            InitializeComponent();

            ruleResultList = new List<SecurityTemplate>();
            ruleBindingList = new BindingList<SecurityTemplate>();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = ruleBindingList;

            //usernameBindingList = new BindingList<string>();
            //UsernameColumn.DataSource = usernameBindingList;

            ValidateData += new CancelEventHandler(WebsiteSecurityEditorControl_ValidateData);
            ReflectFormToData += new DataEditorReflectEventHandler(WebsiteSecurityEditorControl_ReflectFormToData);
            ReflectDataToForm += new DataEditorReflectEventHandler(WebsiteSecurityEditorControl_ReflectDataToForm);

            securityTemplateAccessInfoBindingSource.Add(
                new SecurityTemplateAccessInfo(SecurityTemplateAccess.Allow));

            securityTemplateAccessInfoBindingSource.Add(
                new SecurityTemplateAccessInfo(SecurityTemplateAccess.Deny));
        }

        new public WebsiteEditorForm ParentForm
        {
            get { return (WebsiteEditorForm)base.ParentForm; }
        }

        void WebsiteSecurityEditorControl_ValidateData(object sender, CancelEventArgs e)
        {
            dataGridView.EndEdit();

            if (ruleBindingList.Where(r => string.IsNullOrEmpty(r.RelativePath)).Count() != 0)
            {
                e.Cancel = true;
                MessageBox.Show(
                    "Cannot create a security rule with no relative path.",
                    "Security rule path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (ParentForm.IisSiteEnabled)
            {
                if (!rootReadAccess(SecurityTemplateAccess.Allow))
                {
                    e.Cancel = true;
                    MessageBox.Show(
                        "When IIS is enabled, read access must be " +
                        "allowed to the root relative path (/ or \\).",
                        "Root read denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (rootReadAccess(SecurityTemplateAccess.Deny))
                {
                    e.Cancel = true;
                    MessageBox.Show(
                        "When IIS is enabled, read access cannot be " +
                        "denied to the root relative path (/ or \\).",
                        "Root read denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private bool rootReadAccess(SecurityTemplateAccess access)
        {
            var q = from r in ruleBindingList
                    where r.Access == access
                        && r.Read
                        && isRootPath(r.RelativePath)
                    select r;
            return q.Count() != 0;
        }

        private bool isRootPath(string p)
        {
            return (p == @"\") || (p == "/");
        }

        void WebsiteSecurityEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            int selectedIndex = 0;
            if (dataGridView.SelectedRows.Count != 0)
            {
                selectedIndex = dataGridView.SelectedRows.Cast<DataGridViewRow>().First().Index;
            }

            if (e.GetData<Website>().SecurityArray != null)
            {
                ruleResultList.Clear();
                ruleBindingList.Clear();

                ruleResultList.AddRange(e.GetData<Website>().SecurityArray);
                ruleResultList.ForEach(s => ruleBindingList.Add(s));
                ruleBindingList.ResetBindings();

                selectGridViewRow(selectedIndex);
            }
        }

        void WebsiteSecurityEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            e.GetData<Website>().SecurityArray = ruleResultList.ToArray();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                SecurityTemplate rule = (SecurityTemplate)dataGridView.Rows[e.RowIndex].DataBoundItem;
                if (rule.PendingAction != ChildPendingAction.Create)
                {
                    rule.PendingAction = ChildPendingAction.Update;
                }

                ChangeMade();
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            SecurityTemplate rule = new SecurityTemplate(RhspDataID.Generate());
            rule.PendingAction = ChildPendingAction.Create;
            //rule.Username = usernameBindingList.FirstOrDefault();

            ruleResultList.Add(rule);
            ruleBindingList.Add(rule);
            ruleBindingList.ResetBindings();

            ChangeMade();

            selectGridViewRow(dataGridView.Rows.Cast<DataGridViewRow>().Last().Index);
            dataGridView.BeginEdit(false);
        }

        private void selectGridViewRow(int rowIndex)
        {
            if (rowIndex < dataGridView.Rows.Count)
            {
                dataGridView.ClearSelection();
                dataGridView.CurrentCell = dataGridView[0, rowIndex];
                dataGridView[0, rowIndex].Selected = true;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            dataGridView.SelectedRows.Cast<DataGridViewRow>().ToList().
                ForEach(r => removeRule((SecurityTemplate)r.DataBoundItem));
            ruleBindingList.ResetBindings();
        }

        private void removeRule(SecurityTemplate rule)
        {
            if (rule.PendingAction == ChildPendingAction.Create)
            {
                // Not yet created, so remove from results.
                ruleResultList.Remove(rule);
            }
            else
            {
                rule.PendingAction = ChildPendingAction.Delete;
            }

            ruleBindingList.Remove(rule);
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            SecurityTemplate rule = (SecurityTemplate)row.DataBoundItem;

            if (rule.PendingAction != ChildPendingAction.Create)
            {
                if ((e.ColumnIndex == dataGridView.Columns[RelativePathColumn.Name].Index)
                    || (e.ColumnIndex == dataGridView.Columns[AccessColumn.Name].Index))
                {
                    // Only allow path and access edit if not created.
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            SecurityTemplate rule = (SecurityTemplate)row.DataBoundItem;
            DataGridViewTextBoxCell relativePathCell = (DataGridViewTextBoxCell)row.Cells[RelativePathColumn.Name];
            //DataGridViewComboBoxCell usernameCell = (DataGridViewComboBoxCell)row.Cells[UsernameColumn.Name];
            DataGridViewComboBoxCell accessCell = (DataGridViewComboBoxCell)row.Cells[AccessColumn.Name];

            if (rule.PendingAction != ChildPendingAction.Create)
            {
                relativePathCell.Style.BackColor = SystemColors.Control;
                //usernameCell.Style.BackColor = SystemColors.Control;
                //usernameCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                accessCell.Style.BackColor = SystemColors.Control;
                accessCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            }
        }
    }
}
