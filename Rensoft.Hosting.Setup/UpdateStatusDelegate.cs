using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Setup.Options;

namespace Rensoft.Hosting.Setup
{
    public delegate void UpdateStatusDelegate(SetupOption option, SetupStatus status, string message);
}
