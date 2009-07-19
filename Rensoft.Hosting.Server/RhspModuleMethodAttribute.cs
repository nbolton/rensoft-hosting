using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.Server
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class RhspModuleMethodAttribute : Attribute
    {
    }
}
