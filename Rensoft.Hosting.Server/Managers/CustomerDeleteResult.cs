using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Managers
{
    [DataContract]
    public class CustomerDeleteResult
    {
        [DataMember]
        public List<string> Errors = new List<string>();
    }
}
