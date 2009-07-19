using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Windows.Forms.DataAction;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Windows.Forms.DataViewing;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    public partial class CustomerViewer : LocalDataViewerControl
    {
        private List<Customer> customerList;

        public List<Customer> CustomerList
        {
            get { return customerList; }
        }

        public CustomerViewer()
        {
            InitializeComponent();

            AddListView(listView);

            this.customerList = new List<Customer>();

            New += new EventHandler(CustomerViewer_New);
            Open += new EventHandler(CustomerViewer_Open);
            LoadAsync += new DataActionEventHandler(CustomerViewer_LoadAsync);
            AfterLoadAsync += new DataActionAfterEventHandler(CustomerViewer_AfterLoadAsync);
            DeleteAsync += new DataActionEventHandler(CustomerViewer_DeleteAsync);
            BeforeDeleteAsync += new DataActionBeforeEventHandler(CustomerViewer_BeforeDeleteAsync);
        }

        void CustomerViewer_BeforeDeleteAsync(object sender, DataActionBeforeEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to permenantly delete the selected customers?",
                "Delete customers", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        public delegate DialogResult MessageBoxInvoker();

        void CustomerViewer_DeleteAsync(object sender, DataActionEventArgs e)
        {
            WebsiteAdapter wa = CreateAdapter<WebsiteAdapter>();
            CustomerAdapter ca = CreateAdapter<CustomerAdapter>();
            int[] indexes = e.GetData<DataViewerOpenArgs>().SelectedIndices;

            foreach (Customer customer in indexes.Select(i => customerList[i]))
            {
                bool removeCustomer = true;
                Website[] websites = wa.GetFromCustomer(customer.DataID);
                if (websites.Count() != 0)
                {
                    string websiteNames = string.Join(", ", websites.Select(w => w.Name).ToArray());
                    DialogResult result = (DialogResult)Invoke((MessageBoxInvoker)delegate
                    {
                        return MessageBox.Show(
                            "Removing the customer (" + customer.CodeAndName + ") " +
                            "will remove all associated websites (" + websiteNames + "). " +
                            "Would you like to remove this customer?",
                            "Remove websites",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                    });

                    removeCustomer = (result == DialogResult.Yes);
                }

                if (removeCustomer)
                {
                    CustomerDeleteResult cdr = ca.Delete(customer.DataID);
                    if (cdr.Errors.Length != 0)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show(
                                "One or more errors occured while deleting " +
                                "customer '" + customer.CodeAndName + "'.\r\n\r\n" +
                                string.Join("\r\n", cdr.Errors.Select(error => "> " + error).ToArray()),
                                "Delete customer",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        });
                    }
                }
            }
        }

        void CustomerViewer_New(object sender, EventArgs e)
        {
            Customer customer = new Customer(RhspDataID.Generate());
            CustomerEditorForm editor = new CustomerEditorForm();
            editor.SetCreateMode(customer);
            ShowEditorAsync(editor);
        }

        void CustomerViewer_Open(object sender, EventArgs e)
        {
            Customer customer = customerList[listView.SelectedIndices[0]];
            CustomerEditorForm editor = GetEditor<CustomerEditorForm>(customer);
            editor.SetUpdateMode(customer);
            ShowEditorAsync(editor);
        }

        void CustomerViewer_LoadAsync(object sender, DataActionEventArgs e)
        {
            e.ReplaceData(CreateAdapter<CustomerAdapter>().GetAll());
        }

        void CustomerViewer_AfterLoadAsync(object sender, DataActionAfterEventArgs e)
        {
            lock (customerList)
            {
                customerList.Clear();
                customerList.AddRange(e.GetData<Customer[]>());

                listView.VirtualListSize = customerList.Count;
                listView.Refresh();
            }
        }

        private void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            Customer customer = customerList[e.ItemIndex];

            string[] subItems = new string[] 
            {
                customer.Code,
                customer.Name
            };

            ListViewItem item = new ListViewItem(subItems);
            item.Tag = customer;

            e.Item = item;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedAsync(listView);
        }

        private IEnumerable<Customer> getSelectedCustomers()
        {
            return listView.SelectedIndices.Cast<int>().Select(i => customerList[i]);
        }
    }
}
