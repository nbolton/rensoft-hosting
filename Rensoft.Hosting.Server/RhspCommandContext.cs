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
    public class RhspCommandContext
    {
        private int hostingUserID;

        [DataMember]
        public int HostingUserID
        {
            get { return hostingUserID; }
            set { hostingUserID = value; }
        }
    }
}
