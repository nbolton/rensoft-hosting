using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DebuggerStepThrough]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RhspService : IRhspService
    {
        private RhspServiceContext context;
        private RhspCommandHandler commandHandler;

        public RhspServiceContext Context
        {
            get { return context; }
        }

        public RhspService()
        {
            this.context = new RhspServiceContext();
            this.commandHandler = new RhspCommandHandler(this);
        }

        public RhspCommandResponse GetCommandResponse(RhspCommandCarrier carrier)
        {
            return commandHandler.GetCommandResponse(carrier);
        }
    }
}
