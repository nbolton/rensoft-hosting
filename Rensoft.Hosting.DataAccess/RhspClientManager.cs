using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;
using System.Diagnostics;
using System.ServiceModel.Description;
using System.Net;
using System.ComponentModel;

namespace Rensoft.Hosting.DataAccess
{
    //[DebuggerStepThrough]
    public class RhspClientManager
    {
        public NetTcpBinding Binding { get; set; }
        public EndpointAddress Address { get; set; }
        public RhspCommandContext Context { get; set; }
        public RhspSecurity Security { get; set; }

        private TimeSpan ts;
        public TimeSpan Timeout
        {
            get { return ts; }
            set { ts = value; }
        }

        public RhspClientManager()
        {
            // Set default timeout to 1 minute.
            this.Timeout = new TimeSpan(0, 1, 0);
            this.Context = new RhspCommandContext();
            this.Security = RhspSecurity.Default;
        }

        private RhspServiceClient createClient()
        {
            return new RhspServiceClient(Binding, Address, Security, Timeout);
        }

        private RhspAdapterFactory createAdapterFactory()
        {
            return new RhspAdapterFactory(this, Context);
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            RhspAdapterFactory factory = createAdapterFactory();
            return factory.CreateAdapter<TAdapter>();
        }

        public RhspConnection CreateConnection()
        {
            return new RhspConnection(createClient(), Context);
        }
    }
}
