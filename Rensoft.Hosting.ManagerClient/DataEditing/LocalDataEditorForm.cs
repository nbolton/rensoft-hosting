using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Windows.Forms.DataAction;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public partial class LocalDataEditorForm : Rensoft.Windows.Forms.DataEditing.DataEditorForm
    {
        public LocalDataEditorForm()
        {
            InitializeComponent();
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            return LocalContext.Default.CreateAdapter<TAdapter>();
        }
    }
}
