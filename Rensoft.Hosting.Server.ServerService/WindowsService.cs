using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace Rensoft.Hosting.Server.ServerService
{
    public partial class WindowsService : ServiceBase
    {
        RhspService service;
        ServiceHost host;

        public WindowsService()
        {
            InitializeComponent();

            this.service = new RhspService();
            this.host = new ServiceHost(service);
        }

        protected override void OnStart(string[] args)
        {
            host.Open();
        }

        protected override void OnStop()
        {
            host.Close();
        }
    }
}
