using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataAction;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.ManagerClient.WebsiteWizard;
using System.Diagnostics;
using System.IO;
using Rensoft.Windows.Forms.DataViewing;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    public partial class WebsiteViewer : Rensoft.Hosting.ManagerClient.DataViewing.LocalDataViewerControl
    {
        private List<Website> websiteList;

        public WebsiteViewer()
        {
            InitializeComponent();

            AddListView(listView);

            this.websiteList = new List<Website>();

            New += new EventHandler(WebsiteViewer_New);
            Open += new EventHandler(WebsiteViewer_Open);
            LoadAsync += new DataActionEventHandler(WebsiteViewer_LoadAsync);
            AfterLoadAsync += new DataActionAfterEventHandler(WebsiteViewer_AfterLoadAsync);
            DeleteAsync += new DataActionEventHandler(WebsiteViewer_DeleteAsync);
            BeforeDeleteAsync += new DataActionBeforeEventHandler(WebsiteViewer_BeforeDeleteAsync);
        }

        void WebsiteViewer_BeforeDeleteAsync(object sender, DataActionBeforeEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to permenantly delete the selected websites?",
                "Delete websites", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        void WebsiteViewer_DeleteAsync(object sender, DataActionEventArgs e)
        {
            foreach (int index in e.GetData<DataViewerOpenArgs>().SelectedIndices)
            {
                Website website = websiteList[index];
                WebsiteDeleteResult wdr = CreateAdapter<WebsiteAdapter>().Delete(website.DataID);
                if (wdr.Errors.Length != 0)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show(
                            "One or more errors occured while deleting " +
                            "website '" + website.Name + "'.\r\n\r\n" +
                            string.Join("\r\n", wdr.Errors.Select(error => "> " + error).ToArray()),
                            "Delete website",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    });
                }
            }
        }

        void WebsiteViewer_New(object sender, EventArgs e)
        {
            WebsiteWizardForm wizard = new WebsiteWizardForm(this);
            wizard.FormClosed += new FormClosedEventHandler(wizard_FormClosed);
            wizard.Show();
        }

        void wizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            RunLoadAsync();
        }

        void WebsiteViewer_Open(object sender, EventArgs e)
        {
            Website website = websiteList[listView.SelectedIndices[0]];
            WebsiteEditorForm editor = GetEditor<WebsiteEditorForm>(website);
            editor.SetUpdateMode(website);
            ShowEditorAsync(editor);
        }

        void WebsiteViewer_LoadAsync(object sender, DataActionEventArgs e)
        {
            e.ReplaceData(CreateAdapter<WebsiteAdapter>().GetAll());
        }

        void WebsiteViewer_AfterLoadAsync(object sender, DataActionAfterEventArgs e)
        {
            lock (websiteList)
            {
                websiteList.Clear();
                websiteList.AddRange(e.GetData<Website[]>());

                listView.VirtualListSize = websiteList.Count;
                listView.Refresh();
            }
        }

        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            Website website = websiteList[e.ItemIndex];

            string[] subItems = new string[] 
            {
                //website.PrimaryHost.Name,
                website.Name,
                website.Customer != null ? website.Customer.CodeAndName : "None"
            };

            ListViewItem item = new ListViewItem(subItems);
            item.Tag = website;

            e.Item = item;
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count != 0)
            {
                Website website = websiteList[listView.SelectedIndices[0]];
                Process.Start("http://" + website.PrimaryHost.Name);
            }
        }

        private void openDirectoryAsync()
        {
            if (!openDirectoryBackgroundWorker.IsBusy 
                && (listView.SelectedIndices.Count != 0))
            {
                Guid statusGuid = AsyncStatusChange("Opening website directory...");
                OpenDirectoryArgument argument = new OpenDirectoryArgument() { StatusGuid = statusGuid };
                argument.SelectedWebsite = websiteList[listView.SelectedIndices[0]];
                openDirectoryBackgroundWorker.RunWorkerAsync(argument);
            }
        }

        protected class OpenDirectoryArgument
        {
            public Website SelectedWebsite { get; set; }
            public Guid StatusGuid { get; set; }
        }

        private void openDirectoryBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OpenDirectoryArgument argument = (OpenDirectoryArgument)e.Argument;
            ServerConfigAdapter sca = CreateAdapter<ServerConfigAdapter>();

            OpenDirectoryResult result = new OpenDirectoryResult { StatusGuid = argument.StatusGuid };
            if (sca.WindowsServerName == Environment.MachineName)
            {
                result.DirectoryIsLocal = true;
                result.DirectoryPath = argument.SelectedWebsite.GetDirectory(sca.WebsiteDirectory.FullName);
            }
            e.Result = result;
        }

        protected class OpenDirectoryResult
        {
            public bool DirectoryIsLocal { get; set; }
            public string DirectoryPath { get; set; }
            public Guid StatusGuid { get; set; }
        }

        private void openDirectoryBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OpenDirectoryResult result = (OpenDirectoryResult)e.Result;

            AsyncStatusRevert(result.StatusGuid);

            if (e.Error != null)
            {
                throw new Exception("Could not open website directory.", e.Error);
            }

            if (result.DirectoryIsLocal)
            {
                Process.Start(result.DirectoryPath);
            }
            else
            {
                MessageBox.Show(
                    "The website path is not local to this server (" +
                    Environment.MachineName + ") and so cannot be opened.",
                    "Path not local", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exploreContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDirectoryAsync();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedAsync(listView);
        }
    }
}
