using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess
{
    public class RhspException : Exception
    {
        public RhspException(string message) : base(message) { }
        public RhspException(string message, Exception innerException) : base(message, innerException) { }
    }
}
