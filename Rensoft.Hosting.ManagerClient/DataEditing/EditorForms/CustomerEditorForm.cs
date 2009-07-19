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
using Rensoft.Windows.Forms.DataEditing;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorForms
{
    public partial class CustomerEditorForm : LocalDataEditorForm
    {
        public Customer EditorData
        {
            get { return GetEditorData<Customer>(); }
        }

        public CustomerEditorForm()
        {
            InitializeComponent();
            AddEditorControl(customerGeneralEditorControl1);
            AddEditorControl(customerFtpUsersEditorControl1);

            LoadAsync += new DataActionEventHandler(CustomerEditorForm_LoadAsync);
            CreateAsync += new DataActionEventHandler(CustomerEditorForm_CreateAsync);
            UpdateAsync += new DataActionEventHandler(CustomerEditorForm_UpdateAsync);
            AfterSaveAsync += new DataActionAfterEventHandler(CustomerEditorForm_AfterSaveAsync);
        }

        void CustomerEditorForm_AfterSaveAsync(object sender, DataActionAfterEventArgs e)
        {
            if (EditorData.FtpUserArray != null)
            {
                List<FtpUser> list = EditorData.FtpUserArray.ToList();
                list.ForEach(ftpUser => ftpUser.PendingAction = ChildPendingAction.None);
            }
        }

        void CustomerEditorForm_LoadAsync(object sender, DataActionEventArgs e)
        {
            if (EditorMode == DataEditorMode.Update)
            {
                // Replace stale data with fresh data.
                SetEditorData(CreateAdapter<CustomerAdapter>().Get(GetEditorData<Customer>().DataID));
            }
        }

        void CustomerEditorForm_CreateAsync(object sender, DataActionEventArgs e)
        {
            CustomerAdapter ca = CreateAdapter<CustomerAdapter>();
            if (!ca.ExistsWithCode(GetEditorData<Customer>()))
            {
                ca.Create(GetEditorData<Customer>());
            }
            else
            {
                e.Cancelled = true;
                e.UserMessage = "A customer with the specified code already exists.";
            }
        }

        void CustomerEditorForm_UpdateAsync(object sender, DataActionEventArgs e)
        {
            CustomerAdapter ca = CreateAdapter<CustomerAdapter>();
            if (!ca.ExistsWithCode(GetEditorData<Customer>()))
            {
                ca.Update(GetEditorData<Customer>());
            }
            else
            {
                e.Cancelled = true;
                e.UserMessage = "An other customer with the specified code already exists.";
            }
        }
    }
}
