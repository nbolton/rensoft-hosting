using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.Server.Managers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class RhspManagerUsageAttribute : Attribute
    {
        public string ModuleName { get; protected set; }

        public RhspManagerUsageAttribute(string moduleName)
        {
            this.ModuleName = moduleName;
        }
    }
}
