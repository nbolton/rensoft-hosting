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
    public partial class WebsiteDnsZoneEditorControl : Rensoft.Hosting.ManagerClient.DataEditing.LocalDataEditorControl
    {
        private List<DnsZone> resultList;
        private BindingList<DnsZone> bindingList;

        public WebsiteDnsZoneEditorControl()
        {
            InitializeComponent();

            resultList = new List<DnsZone>();
            bindingList = new BindingList<DnsZone>();

            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = bindingList;

            ReflectDataToForm += new DataEditorReflectEventHandler(WebsiteDnsZoneEditorControl_ReflectDataToForm);
            ReflectFormToData += new DataEditorReflectEventHandler(WebsiteDnsZoneEditorControl_ReflectFormToData);
        }

        void WebsiteDnsZoneEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            if (e.GetData<Website>().DnsZoneArray != null)
            {
                int selectedIndex = getSelectedIndex();

                resultList.Clear();
                bindingList.Clear();

                resultList.AddRange(e.GetData<Website>().DnsZoneArray);
                resultList.ForEach(ftpUser => bindingList.Add(ftpUser));
                bindingList.ResetBindings();

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

        void WebsiteDnsZoneEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            dataGridView.EndEdit();
            e.GetData<Website>().DnsZoneArray = resultList.ToArray();
        }

        private void addbutton_Click(object sender, EventArgs e)
        {
            DnsZone zone = new DnsZone(RhspDataID.Generate());
            zone.PendingAction = ChildPendingAction.Create;

            DnsZoneEditor editor = new DnsZoneEditor();
            editor.ReflectDataToForm(zone);
            DialogResult result = editor.ShowDialog();

            if (result == DialogResult.OK) 
            {
                editor.ReflectFormToData(zone);
                resultList.Add(zone);
                bindingList.Add(zone);
                bindingList.ResetBindings();
                ChangeMade();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count != 0)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete the selected DNS zones?",
                    "Delete DNS zones", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    List<DataGridViewRow> list = dataGridView.SelectedRows.Cast<DataGridViewRow>().ToList();
                    list.ForEach(r => deleteDnsZone((DnsZone)r.DataBoundItem));
                    bindingList.ResetBindings();
                    ChangeMade();
                }
            }
        }

        private void deleteDnsZone(DnsZone dnsZone)
        {
            // Mark deleted so that it is deleted on server request.
            dnsZone.PendingAction = ChildPendingAction.Delete;

            // Remove from visual binding list.
            bindingList.Remove(dnsZone);
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DnsZone dnsZone = (DnsZone)dataGridView.Rows[e.RowIndex].DataBoundItem;
                DnsZoneEditor editor = new DnsZoneEditor();
                editor.ReflectDataToForm(dnsZone);
                DialogResult result = editor.ShowDialog();

                if (result == DialogResult.OK)
                {
                    editor.ReflectFormToData(dnsZone);
                    dnsZone.PendingAction = ChildPendingAction.Update;
                    bindingList.ResetBindings();
                }
            }
        }
    }
}
