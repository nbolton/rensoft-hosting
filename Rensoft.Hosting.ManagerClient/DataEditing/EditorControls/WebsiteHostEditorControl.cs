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
    public partial class WebsiteHostEditorControl : LocalDataEditorControl
    {
        public event EventHandler CellEndEdit;

        private List<WebsiteHost> hostList;
        private BindingList<WebsiteHost> hostBindingList;
        private BindingList<string> ipBindingList;
        private DataEditorMode lastKnownMode;

        public string[] IpAddressArray
        {
            get { return ipBindingList.ToArray(); }
            set
            {
                ipBindingList.Clear();
                value.ToList().ForEach(ip => ipBindingList.Add(ip));
            }
        }

        public IEnumerable<WebsiteHost> Hosts
        {
            get { return hostBindingList; }
        }

        public WebsiteHostEditorControl()
        {
            InitializeComponent();

            hostList = new List<WebsiteHost>();

            hostBindingList = new BindingList<WebsiteHost>();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = hostBindingList;

            ipBindingList = new BindingList<string>();
            IpAddressColumn.DataSource = ipBindingList;

            addWebsiteHostProtocol(WebsiteHostProtocol.Http);
            addWebsiteHostProtocol(WebsiteHostProtocol.Https);

            ReflectDataToForm += new DataEditorReflectEventHandler(WebsiteHostNameEditorControl_ReflectDataToForm);
            ReflectFormToData += new DataEditorReflectEventHandler(WebsiteHostNameEditorControl_ReflectFormToData);
            ValidateData += new CancelEventHandler(WebsiteHostEditorControl_ValidateData);
        }

        private void addWebsiteHostProtocol(WebsiteHostProtocol protocol)
        {
            websiteHostProtocolInfoBindingSource.Add(new WebsiteHostProtocolInfo(protocol));
        }

        void WebsiteHostEditorControl_ValidateData(object sender, CancelEventArgs e)
        {
            dataGridView.EndEdit();

            if (hostBindingList.Count == 0)
            {
                MessageBox.Show(
                    "There must be at least one host name.",
                    "Host names", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Cancel = true;
            }
            //else if (hostBindingList.Count(h => h.Primary) == 0)
            //{
            //    MessageBox.Show(
            //        "There must be at least one primary host.",
            //        "Primary host", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    e.Cancel = true;
            //}
            //else if (hostBindingList.Count(h => h.Primary) > 1)
            //{
            //    MessageBox.Show(
            //        "There cannot be more than one primary host.",
            //        "Primary host", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    e.Cancel = true;
            //}
        }

        void WebsiteHostNameEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            lastKnownMode = e.Mode;

            if (e.GetData<Website>().HostArray != null)
            {
                int selectedIndex = getSelectedIndex();

                hostList.Clear();
                hostList.AddRange(e.GetData<Website>().HostArray);

                hostBindingList.Clear();
                hostList.ForEach(h => hostBindingList.Add(h));
                hostBindingList.ResetBindings();

                selectGridViewRow(selectedIndex);
            }
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

        private int getSelectedIndex()
        {
            int selectedIndex = 0;
            if (dataGridView.SelectedRows.Count != 0)
            {
                selectedIndex = dataGridView.SelectedRows.Cast<DataGridViewRow>().First().Index;
            }
            return selectedIndex;
        }

        void WebsiteHostNameEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            lastKnownMode = e.Mode;
            dataGridView.EndEdit();
            e.GetData<Website>().HostArray = hostList.ToArray();
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CellEndEdit != null) CellEndEdit(this, e);
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ChangeMade();

            if (e.RowIndex != -1)
            {
                WebsiteHost host = (WebsiteHost)dataGridView.Rows[e.RowIndex].DataBoundItem;
                if (host.PendingAction != ChildPendingAction.Create)
                {
                    host.PendingAction = ChildPendingAction.Update;
                }

                if (e.ColumnIndex == dataGridView.Columns[ProtocolColumn.Name].Index)
                {
                    // If protocol changed from http to https.
                    if (host.Protocol == WebsiteHostProtocol.Https)
                    {
                        // Only change if non-custom port.
                        if (host.Port == WebsiteHost.DefaultHttpPort)
                        {
                            host.Port = WebsiteHost.DefaultHttpsPort;

                            // Secure sites are bound to IPs, not hostnames.
                            host.Name = string.Empty;
                        }
                    }
                    else
                    {
                        // Only change if non-custom port.
                        if (host.Port == WebsiteHost.DefaultHttpsPort)
                        {
                            host.Port = WebsiteHost.DefaultHttpPort;
                        }
                    }
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            WebsiteHost host = new WebsiteHost(RhspDataID.Generate());
            host.PendingAction = ChildPendingAction.Create;
            host.Port = WebsiteHost.DefaultHttpPort;
            host.IpAddress = IpAddressArray.FirstOrDefault();

            hostList.Add(host);
            hostBindingList.Add(host);
            hostBindingList.ResetBindings();

            ChangeMade();
            
            selectGridViewRow(dataGridView.Rows.Cast<DataGridViewRow>().Last().Index);
            dataGridView.BeginEdit(false);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> rowList = dataGridView.SelectedRows.Cast<DataGridViewRow>();
            rowList.ToList().ForEach(r => removeWebsiteHost((WebsiteHost)r.DataBoundItem));
            hostBindingList.ResetBindings();
            ChangeMade();
        }

        private void removeWebsiteHost(WebsiteHost host)
        {
            RhspData.SetDeleteOrDiscard(host);
            hostBindingList.Remove(host);
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            DataGridViewTextBoxCell nameCell = (DataGridViewTextBoxCell)row.Cells[NameColumn.Name];

            if ((dataGridView.Columns[e.ColumnIndex].Name == NameColumn.Name) && httpsIsSelected(row))
            {
                e.Cancel = true;
            }
            else
            {
                nameCell.Style.BackColor = SystemColors.Window;
            }

            //if (lastKnownMode == DataEditorMode.Update)
            //{
            //    // Do not allow edit of primary host in update mode.
            //    if (dataGridView.Columns[e.ColumnIndex].Name == PrimaryColumn.Name)
            //    {
            //        e.Cancel = true;
            //    }

            //    // Do not allow host name change of primary host in update mode.
            //    if ((dataGridView.Columns[e.ColumnIndex].Name == NameColumn.Name)
            //        && ((bool)row.Cells[PrimaryColumn.Name].Value))
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private bool httpsIsSelected(DataGridViewRow row)
        {
            DataGridViewCell protocolCell = row.Cells[ProtocolColumn.Name];
            return ((WebsiteHostProtocol)protocolCell.Value) == WebsiteHostProtocol.Https;
        }

        private void dataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            DataGridViewTextBoxCell nameCell = (DataGridViewTextBoxCell)row.Cells[NameColumn.Name];
            WebsiteHost websiteHost = (WebsiteHost)row.DataBoundItem;

            if (websiteHost.Protocol == WebsiteHostProtocol.Https)
            {
                nameCell.Style.BackColor = SystemColors.Control;
            }
            else
            {
                nameCell.Style.BackColor = SystemColors.Window;
            }

            //DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            //WebsiteHost websiteHost = (WebsiteHost)row.DataBoundItem;
            //DataGridViewTextBoxCell nameCell = (DataGridViewTextBoxCell)row.Cells[NameColumn.Name];
            //DataGridViewCheckBoxCell primaryCell = (DataGridViewCheckBoxCell)row.Cells[PrimaryColumn.Name];

            //if (lastKnownMode == DataEditorMode.Update)
            //{
            //    primaryCell.Style.BackColor = SystemColors.Control;
            //    if (websiteHost.Primary)
            //    {
            //        nameCell.Style.BackColor = SystemColors.Control;
            //    }
            //}
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if ((dataGridView.CurrentCell.ColumnIndex == dataGridView.Columns[ProtocolColumn.Name].Index) &&
            //    (e.Control is ComboBox))
            //{
            //    ComboBox protocolComboBox = (ComboBox)e.Control;
            //    protocolComboBox.SelectedIndexChanged += new EventHandler(protocolComboBox_SelectedIndexChanged);
            //}
        }

        void protocolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox comboBox = (ComboBox)sender;
            //DataGridViewRow row = dataGridView.CurrentRow;
            //WebsiteHost websiteHost = (WebsiteHost)row.DataBoundItem;
            //DataGridViewCell nameCell = row.Cells[NameColumn.Name];

            //if ((WebsiteHostProtocol)comboBox.SelectedItem == WebsiteHostProtocol.Https)
            //{
            //    if ((int)nameCell.Value == WebsiteHost.DefaultHttpPort)
            //    {
            //        nameCell.Value = WebsiteHost.DefaultHttpsPort;
            //    }
            //}
        }
    }
}
