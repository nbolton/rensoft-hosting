using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    [DebuggerStepThrough]
    public class RhspCommandCarrier
    {
        private RhspCommand command;
        private RhspCommandContext context;

        [DataMember]
        public RhspCommandContext Context
        {
            get { return context; }
            set { context = value; }
        }

        [DataMember]
        public RhspCommand Command
        {
            get { return command; }
            set { command = value; }
        }
    }
}
