using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class CustomerPage : Rensoft.Hosting.ManagerClient.WebsiteWizard.WebsiteWizardPage
    {
        public CustomerPage()
        {
            InitializeComponent();

            LoadAsync += new DoWorkEventHandler(CustomerPage_LoadAsync);
            AfterLoadAsync += new RunWorkerCompletedEventHandler(CustomerPage_AfterLoadAsync);
            AfterNextAsync += new RunWorkerCompletedEventHandler(CustomerPage_AfterNextAsync);
        }

        void CustomerPage_AfterNextAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            Customer customer = (Customer)customerBindingSource.Current;
            Website.CustomerID = customer.DataID;
            Website.Customer = customer;
        }

        void CustomerPage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            customerBindingSource.ResetBindings(false);
        }

        void CustomerPage_LoadAsync(object sender, DoWorkEventArgs e)
        {
            CustomerAdapter ca = LocalContext.Default.CreateAdapter<CustomerAdapter>();
            customerBindingSource.DataSource = ca.GetAll();
        }
    }
}
