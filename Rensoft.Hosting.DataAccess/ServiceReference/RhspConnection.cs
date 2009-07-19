using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    [DebuggerStepThrough]
    public class RhspConnection
    {
        private RhspServiceClient client;
        private RhspCommandContext context;

        public RhspServiceClient Client
        {
            get { return client; }
        }

        public RhspCommandContext Context
        {
            get { return context; }
        }

        public RhspConnection(RhspServiceClient client, RhspCommandContext context)
        {
            this.client = client;
            this.context = context;
        }
    }
}
