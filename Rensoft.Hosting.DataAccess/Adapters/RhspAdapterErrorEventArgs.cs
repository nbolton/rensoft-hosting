using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    public class RhspAdapterErrorEventArgs : EventArgs
    {
        public bool ThrowException { get; set; }
        public Exception Error { get; set; }
    }
}