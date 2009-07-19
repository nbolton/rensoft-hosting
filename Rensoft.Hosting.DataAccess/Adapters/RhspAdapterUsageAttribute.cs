using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class RhspAdapterUsageAttribute : Attribute
    {
        public string ModuleName { get; private set; }

        public RhspAdapterUsageAttribute(string moduleName)
        {
            this.ModuleName = moduleName;
        }
    }
}
