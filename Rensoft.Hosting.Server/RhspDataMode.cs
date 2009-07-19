using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    public enum RhspDataMode
    {
        [EnumMember]
        Single,

        [EnumMember]
        Array,
    }
}
