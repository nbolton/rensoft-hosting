using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Text.RegularExpressions;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public partial class DnsZoneEditor : Form
    {
        private BindingList<DnsRecord> recordBindingList;
        private List<DnsRecord> recordResultList;

        public DnsZoneEditor()
        {
            InitializeComponent();

            recordResultList = new List<DnsRecord>();
            recordBindingList = new BindingList<DnsRecord>();

            recordDataGridView.AutoGenerateColumns = false;
            recordDataGridView.DataSource = recordBindingList;

            bindRecordTypeInfo();
        }

        private void bindRecordTypeInfo()
        {
            List<DnsRecordTypeInfo> list = new List<DnsRecordTypeInfo>();
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.A, Name = "A" });
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.CNAME, Name = "CNAME" });
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.NS, Name = "NS" });
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.MX, Name = "MX" });
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.TXT, Name = "TXT" });
            list.Add(new DnsRecordTypeInfo { RecordType = DnsRecordType.PTR, Name = "PTR" });
            list.ForEach(ti => dnsRecordTypeInfoBindingSource.Add(ti));
        }

        public void ReflectDataToForm(DnsZone dnsZone)
        {
            nameTextBox.Text = dnsZone.Name;
            defaultTtlTextBox.Text = dnsZone.DefaultTtl;

            if (dnsZone.RecordArray != null)
            {
                var q = from r in dnsZone.RecordArray
                        orderby r.RecordType, r.Name, r.Value, r.Ttl
                        select r;

                //dnsZone.RecordArray.OrderBy(r => r.Name).ToList().ForEach(r => addRecord(r));
                q.ToList().ForEach(r => addRecord(r));
            }

            recordBindingList.ResetBindings();
        }

        private void addRecord(DnsRecord record)
        {
            // Always add to main list even if deleted.
            recordResultList.Add(record);

            // Editor could be re-opened after deletion, so only add visible.
            if (record.PendingAction != ChildPendingAction.Delete)
            {
                recordBindingList.Add(record);
            }
        }

        public void ReflectFormToData(DnsZone dnsZone)
        {
            dnsZone.Name = nameTextBox.Text;
            dnsZone.DefaultTtl = defaultTtlTextBox.Text;
            dnsZone.RecordArray = recordResultList.ToArray();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            recordDataGridView.EndEdit();

            DialogResult = DialogResult.None;
            if (customValidate())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool customValidate()
        {
            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                MessageBox.Show(
                    "A zone name is required.", "Zone name",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(defaultTtlTextBox.Text))
            {
                MessageBox.Show(
                    "A default TTL is required.", "Default TTL",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (recordBindingList.ToList().Count(r => r.RecordType == DnsRecordType.NS) == 0)
            {
                MessageBox.Show(
                    "At least one name server (NS) record is required.",
                    "Zone name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Regex mxRegex = new Regex(@"\d*\s.*");
            var mxQuery = from r in recordBindingList
                    where r.RecordType == DnsRecordType.MX
                    where ((r.Value != null) ? !mxRegex.Match(r.Value).Success : true)
                    select r;

            if (mxQuery.Count() != 0)
            {
                MessageBox.Show(
                    "At least one mail exchange (MX) record value was not in the " +
                    "correct format. MX record values should contain a priority " +
                    "followed by the mail exchange host name (e.g. 10 mx.rensoft.net.).",
                    "Zone name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void recordAddButton_Click(object sender, EventArgs e)
        {
            DnsRecord record = new DnsRecord(RhspDataID.Generate());
            record.PendingAction = ChildPendingAction.Create;

            recordResultList.Add(record);
            recordBindingList.Add(record);
            recordBindingList.ResetBindings();

            selectGridViewRow(recordDataGridView.Rows.Cast<DataGridViewRow>().Last().Index);
            recordDataGridView.BeginEdit(false);
        }

        private void selectGridViewRow(int rowIndex)
        {
            if (rowIndex < recordDataGridView.Rows.Count)
            {
                recordDataGridView.ClearSelection();
                recordDataGridView.CurrentCell = recordDataGridView[0, rowIndex];
                recordDataGridView[0, rowIndex].Selected = true;
            }
        }

        private void recordRemoveButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in recordDataGridView.SelectedRows)
            {
                DnsRecord r = (DnsRecord)row.DataBoundItem;

                if (r.PendingAction == ChildPendingAction.Create)
                {
                    // Don't send to server, as all items are used to regenerate config.
                    recordResultList.Remove(r);
                }
                else
                {
                    r.PendingAction = ChildPendingAction.Delete;
                }

                // Remove from visible list but keep in result list.
                recordBindingList.Remove(r);
            }
            recordBindingList.ResetBindings();
        }

        private void recordDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DnsRecord record = (DnsRecord)recordDataGridView.Rows[e.RowIndex].DataBoundItem;
                if (record.PendingAction != ChildPendingAction.Create)
                {
                    record.PendingAction = ChildPendingAction.Update;
                }
            }
        }
    }
}
