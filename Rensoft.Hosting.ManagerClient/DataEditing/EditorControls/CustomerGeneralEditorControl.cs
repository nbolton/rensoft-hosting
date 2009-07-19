using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataEditing;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    public partial class CustomerGeneralEditorControl : LocalDataEditorControl
    {
        public CustomerGeneralEditorControl()
        {
            InitializeComponent();

            ReflectDataToForm += new DataEditorReflectEventHandler(CustomerEditorControl_ReflectDataToForm);
            ReflectFormToData += new DataEditorReflectEventHandler(CustomerEditorControl_ReflectFormToData);
        }

        void CustomerEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            Customer customer = e.GetData<Customer>();
            codeTextBox.Text = customer.Code;
            nameTextBox.Text = customer.Name;

            if (e.Mode == DataEditorMode.Update)
            {
                codeTextBox.ReadOnly = true;
            }
        }

        void CustomerEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            Customer customer = e.GetData<Customer>();
            customer.Code = codeTextBox.Text;
            customer.Name = nameTextBox.Text;
        }

        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {
            ChangeMade();
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            ChangeMade();
        }
    }
}
