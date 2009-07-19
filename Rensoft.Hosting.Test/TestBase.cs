using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.DataAccess.Adapters;
using System.Diagnostics;

namespace Rensoft.Hosting.Test
{
    [DebuggerStepThrough]
    public class TestBase
    {
        public TestContext TestContext { get; set; }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            return LocalContext.Default.CreateAdapter<TAdapter>();
        }
    }
}
