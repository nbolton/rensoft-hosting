﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public enum WebsiteIisMode
    {
        [EnumMember]
        Disabled,

        [EnumMember]
        Standard,

        [EnumMember]
        Redirect
    }
}
