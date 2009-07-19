using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Reflection;
using System.Diagnostics;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [DebuggerStepThrough]
    public class RhspAdapterFactory
    {
        private RhspClientManager manager;
        private RhspCommandContext context;

        public RhspAdapterFactory(RhspClientManager manager, RhspCommandContext context)
        {
            this.manager = manager;
            this.context = context;
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            ConstructorInfo constructor = typeof(TAdapter).GetConstructor(new Type[] { });
            if (constructor == null)
            {
                throw new InvalidOperationException(
                    "Unable to find empty public constructor for '" + typeof(TAdapter).Name + "'");
            }

            TAdapter adapter = (TAdapter)constructor.Invoke(new object[] { });
            adapter.Initialize(manager);
            return (TAdapter)adapter;
        }
    }
}
