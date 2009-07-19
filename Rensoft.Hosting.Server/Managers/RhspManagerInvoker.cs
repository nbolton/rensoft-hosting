using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Rensoft.Hosting.Server.Managers
{
    public class RhspManagerInvoker
    {
        private RhspManager manager;
        private MethodInfo method;
        private object[] parameters;

        [DebuggerStepThrough]
        public RhspManagerInvoker(RhspManager manager, MethodInfo method, object[] parameters)
        {
            this.manager = manager;
            this.method = method;
            this.parameters = parameters;
        }

        [DebuggerStepThrough]
        public object Invoke()
        {
            try
            {
                return method.Invoke(manager, parameters);
            }
            catch (TargetInvocationException ex)
            {
                // Re-throw exception within this exception (ignore target invocation).
                throw new RhspException(ex.InnerException.Message, ex.InnerException);
            }
        }
    }
}
