using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("Diagnostic")]
    public class DiagnosticManager : RhspManager
    {
        [RhspModuleMethod]
        public string Test()
        {
            return "Hello world!";
        }

        [RhspModuleMethod]
        public string Test(string test)
        {
            return "You said: " + test;
        }

        [RhspModuleMethod]
        public string Test(int test2, string test1)
        {
            return "You said: " + test1 + " " + test2;
        }

        [RhspModuleMethod]
        public string Test(string[] testArray)
        {
            return "You said: " + string.Join(" ", testArray);
        }

        [RhspModuleMethod]
        public string[] TestArray()
        {
            return new string[] { "Hello", "World" };
        }

        [RhspModuleMethod]
        public string TestInvalidArray(RhspData[] array)
        {
            // Should fail; used to test exception.
            return string.Empty;
        }

        public List<string> TestInvalidList()
        {
            // Should fail; used to test exception.
            return new List<string>();
        }
    }
}
