using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class ServerStatusElement
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string ActionText { get; set; }

        [DataMember]
        public string ActionCommand { get; set; }

        [DataMember]
        public ServerStatusCondition Condition { get; set; }
    }
}
