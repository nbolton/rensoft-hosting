using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Windows.Forms.DataAction;
using Rensoft.Windows.Forms.DataViewing;
using Rensoft.Hosting.ManagerClient.DnsImportWizard;
using Rensoft.Hosting.ManagerClient.DataViewing;

namespace Rensoft.Hosting.ManagerClient
{
    public partial class MainScreen : Form
    {
        private Dictionary<Guid, DataActionStatusState> statusTable;
        private List<DataViewerControl> viewerList;

        delegate string GSDelegate();
        delegate void SSDelegate();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Status
        {
            get
            {
                return (string)Invoke((GSDelegate)delegate() { return statusLabel.Text; });
            }
            set
            {
                Invoke((SSDelegate)delegate() { statusLabel.Text = value; });
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DefaultStatus { get; set; }

        public MainScreen()
        {
            InitializeComponent();

            this.viewerList = new List<DataViewerControl>();
            this.statusTable = new Dictionary<Guid, DataActionStatusState>();
            this.DefaultStatus = "Ready";

            AddViewer(serverViewer1);
            AddViewer(customerViewer1);
            AddViewer(websiteViewer1);

            Load += new EventHandler(MainScreen_Load);
            customerViewer1.AfterLoadAsync += new DataActionAfterEventHandler(customerViewer1_AfterLoadAsync);
        }

        void MainScreen_Load(object sender, EventArgs e)
        {
            // Load so website create button is enabled.
            customerViewer1.RunLoadAsync();
        }

        void customerViewer1_AfterLoadAsync(object sender, DataActionAfterEventArgs e)
        {
            websiteToolStripMenuItem.Enabled = (customerViewer1.CustomerList.Count != 0);
        }

        public void AddViewer(LocalDataViewerControl viewer)
        {
            viewer.StatusChange += new StatusChangeEventHandler(viewer_StatusChange);
            viewer.StatusRevert += new StatusRevertEventHandler(viewer_StatusRevert);
            viewer.Shown += new EventHandler(viewer_Shown);
            viewer.ErrorOccured += new ViewerErrorEventHandler(viewer_ErrorOccured);
            viewerList.Add(viewer);
        }

        void viewer_ErrorOccured(object sender, ViewerErrorEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(
                    this,
                    "An error occured while communicating with server.\r\n\r\n" +
                    e.Error.Message,
                    "Error occured",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }

        //private string getRecursiveExceptionMessage(Exception rootException)
        //{
        //    Exception nextException = rootException;
        //    int indent = 0;
        //    string result = string.Empty;

        //    while (nextException != null)
        //    {
        //        /* Build up a recursive list of exception messages, at each tier
        //         * increase the indentation size to make the recursion more apparent. */
        //        result += "> ".PadLeft(3 + indent++, '-') + nextException.Message + "\r\n";
        //        nextException = nextException.InnerException;
        //    }

        //    return result;
        //}

        void viewer_Shown(object sender, EventArgs e)
        {
            ((DataViewerControl)sender).RunLoadAsync();
        }

        void viewer_StatusChange(object sender, StatusChangeEventArgs e)
        {
            e.StatusGuid = AsyncStatusChange(e.StatusText, e.Cursor);
        }

        void viewer_StatusRevert(object sender, StatusRevertEventArgs e)
        {
            AsyncStatusRevert(e.StatusGuid, e.RevertStatus);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            customerViewer1.RunNewAsync();
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            websiteViewer1.RunNewAsync();
        }

        public Guid AsyncStatusChange(string statusText)
        {
            return AsyncStatusChange(statusText, Cursors.WaitCursor);
        }

        public Guid AsyncStatusChange(string statusText, Cursor cursor)
        {
            if (!this.IsHandleCreated)
            {
                throw new InvalidOperationException(
                    "Cannot change status when window handle is not created.");
            }

            Guid guid = Guid.NewGuid();
            DataActionStatusState state = new DataActionStatusState(statusText, cursor);
            statusTable.Add(guid, state);
            Status = statusText;
            Cursor = cursor;
            return guid;
        }

        public void AsyncStatusRevert(Guid statusGuid)
        {
            AsyncStatusRevert(statusGuid, DefaultStatus);
        }

        public void AsyncStatusRevert(Guid statusGuid, string revertStatus)
        {
            if (!this.IsHandleCreated)
            {
                throw new InvalidOperationException(
                    "Cannot revert status when window handle is not created.");
            }

            statusTable.Remove(statusGuid);
            if (statusTable.Count != 0)
            {
                DataActionStatusState last = statusTable.Last().Value;
                Status = last.Message;
                Cursor = last.Cursor;
            }
            else
            {
                Status = string.IsNullOrEmpty(revertStatus) ? DefaultStatus : revertStatus;
                Cursor = Cursors.Default;
            }
        }

        private void importMsDnsZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DnsImportWizardForm form = new DnsImportWizardForm();
            form.ImportMode = DnsImportWizardMode.MicrosoftDnsZones;
            form.Show();
        }

        private void importRensoftDnsZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DnsImportWizardForm form = new DnsImportWizardForm();
            form.ImportMode = DnsImportWizardMode.RensoftDnsZones;
            form.Show();
        }
    }
}
