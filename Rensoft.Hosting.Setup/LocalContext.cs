using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.ServiceModel;
using Rensoft.Hosting.Setup.Properties;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.Setup
{
    public class LocalContext
    {
        public static readonly LocalContext Default = new LocalContext();

        public RhspClientManager GetManager()
        {
            RhspCommandContext context = new RhspCommandContext();
            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(Settings.Default.HostingServiceUri);

            RhspClientManager manager = new RhspClientManager()
            {
                Binding = binding,
                Address = address,
                Context = context
            };
            return manager;
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            return GetManager().CreateAdapter<TAdapter>();
        }
    }
}
