using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Diagnostics;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [DebuggerStepThrough]
    public class DiagnosticAdapter : RhspAdapter
    {
        public ConnectionTestResult TestConnection()
        {
            // Consider making this test a bit more elegant.
            try
            {
                RhspConnection connecton = CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
                RhspCommandResponse response = command.Execute();

                return new ConnectionTestResult()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ConnectionTestResult()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public string Test()
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
            RhspCommandResponse response = command.Execute();
            return response.GetResult<string>();
        }

        public string Test(string test)
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
            command.Parameters.Add("test", test);
            RhspCommandResponse response = command.Execute();
            return response.GetResult<string>();
        }

        public string Test(string test1, int test2)
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
            command.Parameters.Add("test1", test1);
            command.Parameters.Add("test2", test2);
            RhspCommandResponse response = command.Execute();
            return response.GetResult<string>();
        }

        public string Test(string[] testArray)
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
            command.Parameters.Add("testArray", testArray);
            RhspCommandResponse response = command.Execute();
            return response.GetResult<string>();
        }

        public string[] TestArray()
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "Diagnostic.TestArray");
            RhspCommandResponse response = command.Execute();
            return response.GetResult<string[]>();
        }
    }
}
