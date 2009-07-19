using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.Setup.Properties;
using System.Diagnostics;
using System.ServiceProcess;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess;
using System.ServiceModel;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Microsoft.Win32;
using WindowsInstaller;
using Rensoft.Hosting.Setup.Options;
using System.IO;
using System.Linq;

namespace Rensoft.Hosting.Setup
{
    public partial class InstallationPage : SetupWizardPage
    {
        //private List<SetupOption> optionList;

        public InstallationPage()
        {
            InitializeComponent();

            //this.optionList = new List<SetupOption>();
            this.EnableNext = false;

            BeforeLoadAsync += new DoWorkEventHandler(ProgressPage_BeforeLoadAsync);
            LoadAsync += new DoWorkEventHandler(ProgressPage_LoadAsync);
            AfterLoadAsync += new RunWorkerCompletedEventHandler(ProgressPage_AfterLoadAsync);
        }

        void ProgressPage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new Exception("Unable to process install page.", e.Error);
            }

            this.EnableNext = true;
        }

        void ProgressPage_LoadAsync(object sender, DoWorkEventArgs e)
        {
            //for (int i = 0; i < OptionList.Count; i++)
            OptionList.Where(o => o.Enabled).ToList().ForEach(o => processOption(o));
        }

        private void processOption(SetupOption option)
        {
            updateStatus(option, SetupStatus.InProgress);

            if (option.ProcessOption(new UpdateStatusDelegate(updateStatus)))
            {
                // Once all steps complete, mark success.
                updateStatus(option, SetupStatus.Success);
            }
        }

        void ProgressPage_BeforeLoadAsync(object sender, DoWorkEventArgs e)
        {
            //optionList.Clear();

            //if (((SetupWizardForm)ParentWizard).InstallServer)
            //{
            //    optionList.Add(((SetupWizardForm)ParentWizard).ServerOption);
            //}

            //if (((SetupWizardForm)ParentWizard).InstallManager)
            //{
            //    optionList.Add(((SetupWizardForm)ParentWizard).ManagerOption);
            //}

            dataGridView.Rows.Clear();
            foreach (SetupOption option in OptionList.Where(o => o.Enabled))
            {
                option.StatusIndex = dataGridView.Rows.Add(
                    Resources.Pending,
                    option.ProductName,
                    option.Status.ToString(),
                    option.Message);
            }
        }

        private void updateStatus(SetupOption option, SetupStatus status)
        {
            updateStatus(option, status, null);
        }

        private void updateStatus(SetupOption option, SetupStatus status, string message)
        {
            DataGridViewRow row = dataGridView.Rows[option.StatusIndex];
            switch (status)
            {
                case SetupStatus.InProgress:
                    row.Cells[0].Value = Resources.InProgress;
                    row.Cells[2].Value = "In Progress";
                    row.Cells[3].Value = message;
                    break;

                case SetupStatus.Success:
                    row.Cells[0].Value = Resources.Success;
                    row.Cells[2].Value = "Success";
                    row.Cells[3].Value = message;
                    break;

                case SetupStatus.Failure:
                    row.Cells[0].Value = Resources.Failure;
                    row.Cells[2].Value = "Failed";
                    row.Cells[3].Value = message;
                    break;
            }
        }
    }
}