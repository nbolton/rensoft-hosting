using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    public enum ChildPendingAction
    {
        [EnumMember]
        None,

        [EnumMember]
        Create,

        [EnumMember]
        Update,

        [EnumMember]
        Delete,

        [EnumMember]
        Discard
    }
}
