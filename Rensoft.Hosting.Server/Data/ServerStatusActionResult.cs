using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class ServerStatusActionResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string UserMessage { get; set; }
    }
}
