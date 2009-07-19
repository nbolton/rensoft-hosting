using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.Test
{
    [TestClass]
    public class ServerConfigTests : TestBase
    {
        [TestMethod]
        public void LegacyModeTest()
        {
            ServerConfigAdapter adapter = CreateAdapter<ServerConfigAdapter>();

            // Remember the original result for comparison.
            bool originalResult = adapter.EnableLegacyMode;

            // Set the mode to the opposite of what it was.
            adapter.EnableLegacyMode = !originalResult;

            // Ensure that it has in fact changed.
            if (adapter.EnableLegacyMode == originalResult)
            {
                Assert.Fail("Legacy mode valid was unchanged.");
            }
        }
    }
}
