using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Rensoft.Hosting.Test.Properties;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess;
using System.Diagnostics;
using Rensoft.Hosting.Server.Managers;
using Rensoft.Hosting.Server;

namespace Rensoft.Hosting.Test
{
    [DebuggerStepThrough]
    public class LocalContext
    {
        private const string configDirectory =
            @"C:\Users\Nick\Documents\Visual Studio 2008\Projects\Rensoft.Hosting\Rensoft.Hosting.Server.ServerConsole\bin\Debug";

        private RhspClientManager manager;

        public static readonly LocalContext Default = new LocalContext();

        private LocalContext()
        {
            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(Settings.Default.HostingServiceUri);
            
            this.manager = new RhspClientManager()
            {
                Binding = binding,
                Address = address,
                Timeout = new TimeSpan(0, 10, 0)
            };

            initializeContext();
        }

        private void initializeContext()
        {
            manager.Context.HostingUserID = Settings.Default.HostingUserID;
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            return manager.CreateAdapter<TAdapter>();
        }

        public RhspConnection CreateConnection()
        {
            return manager.CreateConnection();
        }

        public TManager CreateManager<TManager>()
             where TManager : RhspManager
        {
            RhspServiceContext context = new RhspServiceContext();
            context.ConfigDirectory = configDirectory;
            return RhspManager.CreateManager<TManager>(context);
        }
    }
}
