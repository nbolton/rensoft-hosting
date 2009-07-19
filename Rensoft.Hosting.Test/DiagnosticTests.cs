using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.Test
{
    [TestClass]
    public class DiagnosticTests : TestBase
    {
        [TestMethod]
        public void ResultAndParamTest()
        {
            DiagnosticAdapter manager = CreateAdapter<DiagnosticAdapter>();

            string result1 = manager.Test();
            string result2 = manager.Test("Hello world!");
            string result3 = manager.Test("Testing", 1234);
            string result4 = manager.Test(new string[] { "Test1", "Test2" });
            string[] result6 = manager.TestArray();
        }

        [TestMethod]
        public void NonExistantMethodTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.DoesNotExist");
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due to non existant method.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "No methods exist with the name 'DoesNotExist'.");
            }
        }

        [TestMethod]
        public void InvalidParamCountTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
                command.Parameters.Add("1", null);
                command.Parameters.Add("2", null);
                command.Parameters.Add("3", null);
                command.Parameters.Add("4", null);
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due to invalid param count.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "None of the methods named 'Test' have the same number of prameters as specified in the command.");
            }
        }

        [TestMethod]
        public void InvalidParamTypeTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.Test");
                command.Parameters.Add("test", true);
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due to param type.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "The data type 'String' of system parameter with name 'test' is not compatible with the equivilent command parameter of type 'Boolean'.");
            }
        }

        [TestMethod]
        public void InvalidListTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.TestInvalidList");
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due invalid return type of list.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Lists are not supported. Consider converting the list to an array.");
            }
        }

        [TestMethod]
        public void InvalidNonArrayTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.TestInvalidArray");
                RhspParameter param = command.Parameters.Add("array", new RhspData[0]);
                param.DataTypeName = "System.String"; // Purposely set non-array type.
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due invalid param array type.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "A non-array type was specified, which is not allowed for array values.");
            }
        }

        [TestMethod]
        public void InvalidNonSystemArrayTest()
        {
            try
            {
                RhspConnection connecton = LocalContext.Default.CreateConnection();
                RhspCommand command = new RhspCommand(connecton, "Diagnostic.TestInvalidArray");
                command.Parameters.Add("array", new RhspData[0]);
                RhspCommandResponse response = command.Execute();
                Assert.Fail("Exception should have been thrown due invalid param array type.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Cannot handle arrays of non-system type. Consider wrapping non-system elements in a system type array such as object[].");
            }
        }
    }
}
