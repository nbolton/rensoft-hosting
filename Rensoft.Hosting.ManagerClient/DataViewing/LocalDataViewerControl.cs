using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Windows.Forms.DataAction;

namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    public partial class LocalDataViewerControl : Rensoft.Windows.Forms.DataViewing.DataViewerControl
    {
        public event ViewerErrorEventHandler ErrorOccured;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(false)]
        public bool AutoChangeStatus { get; set; }

        public LocalDataViewerControl()
        {
            InitializeComponent();

            AutoChangeStatus = true;

            BeforeLoadAsync += new DataActionBeforeEventHandler(LocalDataViewerControl_BeforeLoadAsync);
            AfterLoadAsync += new DataActionAfterEventHandler(LocalDataViewerControl_AfterLoadAsync);
            BeforeDeleteAsync += new DataActionBeforeEventHandler(LocalDataViewerControl_BeforeDeleteAsync);
            AfterDeleteAsync += new DataActionAfterEventHandler(LocalDataViewerControl_AfterDeleteAsync);
        }

        void LocalDataViewerControl_BeforeDeleteAsync(object sender, DataActionBeforeEventArgs e)
        {
            if (AutoChangeStatus)
            {
                e.StatusGuid = AsyncStatusChange("Deleting...");
            }
        }

        void LocalDataViewerControl_AfterDeleteAsync(object sender, DataActionAfterEventArgs e)
        {
            if (AutoChangeStatus)
            {
                AsyncStatusRevert(e.StatusGuid);
            }
        }

        void LocalDataViewerControl_AfterLoadAsync(object sender, DataActionAfterEventArgs e)
        {
            if (AutoChangeStatus)
            {
                AsyncStatusRevert(e.StatusGuid);
            }
        }

        void LocalDataViewerControl_BeforeLoadAsync(object sender, DataActionBeforeEventArgs e)
        {
            if (AutoChangeStatus)
            {
                e.StatusGuid = AsyncStatusChange("Loading...");
            }
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            TAdapter adapter = LocalContext.Default.CreateAdapter<TAdapter>();
            adapter.ErrorOccured += new RhspAdapterErrorEventHandler(adapter_ErrorOccured);
            return adapter;
        }

        void adapter_ErrorOccured(object sender, RhspAdapterErrorEventArgs e)
        {
            e.ThrowException = false;
            if (ErrorOccured != null) ErrorOccured(this, new ViewerErrorEventArgs { Error = e.Error });
        }

        private void refreshLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RunLoadAsync();
        }
    }
}
