using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataAction;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    public partial class ServerStatusViewer : LocalDataViewerControl
    {
        BindingList<ServerStatusElement> bindingList;

        public ServerStatusViewer()
        {
            InitializeComponent();

            AutoChangeStatus = false;

            bindingList = new BindingList<ServerStatusElement>();
            serverDataGridView.AutoGenerateColumns = false;
            serverDataGridView.DataSource = bindingList;

            BeforeLoadAsync += new DataActionBeforeEventHandler(ServerStatusViewer_BeforeLoadAsync);
            LoadAsync += new DataActionEventHandler(ServerViewer_LoadAsync);
            AfterLoadAsync += new DataActionAfterEventHandler(ServerStatusViewer_AfterLoadAsync);
            OpenAsync += new DataActionEventHandler(ServerStatusViewer_OpenAsync);
            AfterOpenAsync += new DataActionAfterEventHandler(ServerStatusViewer_AfterOpenAsync);
            BeforeOpenAsync += new DataActionBeforeEventHandler(ServerStatusViewer_BeforeOpenAsync);
        }

        void ServerStatusViewer_BeforeLoadAsync(object sender, DataActionBeforeEventArgs e)
        {
            e.StatusGuid = AsyncStatusChange("Refreshing status...");
        }

        void ServerStatusViewer_BeforeOpenAsync(object sender, DataActionBeforeEventArgs e)
        {
            e.StatusGuid = AsyncStatusChange("Running action...");
            serverDataGridView.Enabled = false;
        }

        void ServerStatusViewer_AfterOpenAsync(object sender, DataActionAfterEventArgs e)
        {
            AsyncStatusRevert(e.StatusGuid);
            serverDataGridView.Enabled = true;

            RunLoadAsync();

            ServerStatusActionResult result = e.GetData<ServerStatusActionResult>();
            MessageBoxIcon icon;
            if (result.Success)
            {
                icon = MessageBoxIcon.Information;
            }
            else 
            {
                icon = MessageBoxIcon.Warning;
            }

            MessageBox.Show(this, result.UserMessage, "Action result", MessageBoxButtons.OK, icon);
        }

        void ServerStatusViewer_OpenAsync(object sender, DataActionEventArgs e)
        {
            ServerStatusElement element = e.GetData<ServerStatusElement>();
            e.ReplaceData(CreateAdapter<ServerStatusAdapter>().RunActionCommand(element.ActionCommand));
        }

        void ServerStatusViewer_AfterLoadAsync(object sender, DataActionAfterEventArgs e)
        {
            bindingList.Clear();
            e.GetData<ServerStatusElement[]>().ToList().ForEach(s => bindingList.Add(s));
            bindingList.ResetBindings();

            AsyncStatusRevert(e.StatusGuid);
        }

        void ServerViewer_LoadAsync(object sender, DataActionEventArgs e)
        {
            e.ReplaceData(CreateAdapter<ServerStatusAdapter>().GetElementArray());
        }

        private void serverDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = serverDataGridView.Rows[e.RowIndex];
            ServerStatusElement element = (ServerStatusElement)row.DataBoundItem;
            DataGridViewTextBoxCell valueCell = (DataGridViewTextBoxCell)row.Cells[ValueColumn.Name];

            switch (element.Condition)
            {
                case ServerStatusCondition.Normal:
                    valueCell.Style.ForeColor = Color.Green;
                    break;

                case ServerStatusCondition.Error:
                    valueCell.Style.ForeColor = Color.Red;
                    break;
            }
        }

        private void serverDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = serverDataGridView.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            ServerStatusElement element = (ServerStatusElement)row.DataBoundItem;

            if (cell.ColumnIndex == serverDataGridView.Columns[ActionColumn.Name].Index)
            {
                if (!string.IsNullOrEmpty(element.ActionCommand))
                {
                    RunOpenAsync(element);
                }
            }
        }

        private void serverDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // Disable selection.
            serverDataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(r => r.Selected = false);
        }
    }
}
