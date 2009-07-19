using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DebuggerStepThrough]
    public class RhspException : Exception
    {
        public RhspException(string message) : base(message) { }
        public RhspException(string message, Exception innerException) : base(message, innerException) { }
    }
}
