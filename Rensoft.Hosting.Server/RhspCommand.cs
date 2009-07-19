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
    public class RhspCommand
    {
        private string commandText;
        private RhspParameterCollection parameters;

        [DataMember]
        public string CommandText
        {
            get { return commandText; }
            set { commandText = value; }
        }

        [DataMember]
        public RhspParameterCollection Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
