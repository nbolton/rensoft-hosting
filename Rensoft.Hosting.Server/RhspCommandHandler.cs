using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Rensoft.Hosting.Server.Managers;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DebuggerStepThrough]
    public class RhspCommandHandler
    {
        private RhspService service;
        private Dictionary<string, Type> moduleManagerTable;

        public RhspService Service
        {
            get { return service; }
        }

        public RhspCommandHandler(RhspService service)
        {
            this.service = service;

            initializeModuleManagers();
        }

        private void initializeModuleManagers()
        {
            // Add all types from the Managers namespace where class has RhspModuleAttribute.
            moduleManagerTable = new Dictionary<string, Type>();
            foreach (Type type in Assembly.GetCallingAssembly().GetTypes())
            {
                var rmQuery = from rm in type.GetCustomAttributes(false)
                              where ((Attribute)rm).GetType() == typeof(RhspManagerUsageAttribute)
                              select rm;

                if (rmQuery.Count() != 0)
                {
                    // Index the module manager by module name.
                    RhspManagerUsageAttribute rm = (RhspManagerUsageAttribute)rmQuery.First();
                    moduleManagerTable.Add(rm.ModuleName, type);
                }
            }
        }

        public RhspCommandResponse GetCommandResponse(RhspCommandCarrier carrier)
        {
            try
            {
                return getCommandResponse(carrier);
            }
            catch (Exception ex)
            {
                // Save full error info to log for further diagnosis.
                writeErrorToEventLog(ex);

                // Re-throw error so end-user sees message.
                throw new Exception(ex.Message);
            }
        }

        private void writeErrorToEventLog(Exception ex)
        {
            string name = "Rensoft Hosting Server";
            string source = "RensoftHostingService";

            EventLog eventLog = new EventLog(name, ".", source);
            if (!EventLog.Exists(name))
            {
                EventLog.CreateEventSource(source, name);
            }
            eventLog.WriteEntry(getExceptionDetail(ex), EventLogEntryType.Error);
        }

        private string getExceptionDetail(Exception ex)
        {
            List<string> list = new List<string>();
            Exception nextException = ex;
            
            while (nextException != null)
            {
                list.Add(nextException.Message + "\r\n" + nextException.StackTrace);
                nextException = nextException.InnerException;
            }

            return string.Join("\r\n\r\n", list.ToArray());
        }

        private RhspCommandResponse getCommandResponse(RhspCommandCarrier carrier)
        {
            RhspCommandResponse response = new RhspCommandResponse();

            string[] ctParts = carrier.Command.CommandText.Split('.');
            if (ctParts.Length != 2)
            {
                throw new RhspException(
                    "Unable to process command because it is not in " +
                    "the correct format (ModuleName.MethodName).");
            }

            string moduleName = ctParts[0];
            string methodName = ctParts[1];

            if (!moduleManagerTable.ContainsKey(moduleName))
            {
                throw new RhspException(
                    "Unable to process command because the module " +
                    "name '" + moduleName + "' was not recognised.");
            }

            Type managerType = moduleManagerTable[moduleName];

            RhspManager manager = RhspManager.CreateManager(
                service.Context,
                managerType);

            MethodInfo methodInfo = getModuleMethod(
                manager,
                methodName,
                carrier.Command.Parameters);

            ParameterInfo[] systemParamArray = methodInfo.GetParameters();
            object[] methodValueArray = new object[systemParamArray.Length];

            foreach (ParameterInfo systemParam in systemParamArray)
            {
                RhspParameter rhspParam = carrier.Command.Parameters[systemParam.Name];
                if (!systemParam.ParameterType.IsAssignableFrom(rhspParam.Value.GetType()))
                {
                    throw new RhspException(
                        "The data type '" + systemParam.ParameterType.Name + "' " +
                        "of system parameter with name '" + systemParam.Name + "' " +
                        "is not compatible with the equivilent command parameter " +
                        "of type '" + rhspParam.Value.GetType().Name + "'.");
                }

                // Assign the value to the correct position so it matches the signature.
                methodValueArray[systemParam.Position] = rhspParam.Value;
            }

            // Call method using manager so that apropriate events are fired.
            response.SetData(manager.InvokeMethod(methodInfo, methodValueArray));

            // Changes may have been made, so save.
            service.Context.HostingConfig.Save();
            service.Context.ServerConfig.Save();

            return response;
        }

        private MethodInfo getModuleMethod(
            RhspManager manager, 
            string methodName, 
            RhspParameterCollection rhspParams)
        {
            var nameQuery = from m in manager.GetType().GetMethods()
                            where m.Name == methodName
                            select m;

            if (nameQuery.Count() == 0)
            {
                throw new RhspException(
                    "No methods exist with the name '" + methodName + "'.");
            }

            var paramCountQuery = from m in nameQuery
                                  where m.GetParameters().Length == rhspParams.Count
                                  select m;

            if (paramCountQuery.Count() == 0)
            {
                throw new RhspException(
                    "None of the methods named '" + methodName + "' have " +
                    "the same number of prameters as specified in the command.");
            }

            var paramNameQuery = from m in paramCountQuery
                                 where methodHasParamNames(m, rhspParams)
                                 select m;

            if (paramNameQuery.Count() == 0)
            {
                throw new RhspException(
                    "The method with name '" + methodName + "' has the " +
                    "correct number of parameters, but at least one of " +
                    "either the parameter names or types do not match.");
            }

            var moduleMethodQuery = from m in paramNameQuery
                                    where hasModuleMethodAttribute(m)
                                    select m;

            if (moduleMethodQuery.Count() == 0)
            {
                throw new RhspException(
                    "The method with name '" + methodName + "' has the correct number " +
                    "of parameters and the correct parameter types, but does not implement " +
                    "the attribute " + typeof(RhspModuleMethodAttribute).FullName + ".");
            }

            return moduleMethodQuery.Single();
        }

        private bool hasModuleMethodAttribute(MethodInfo m)
        {
            return m.GetCustomAttributes(typeof(RhspModuleMethodAttribute), false).Length != 0;
        }

        private bool methodHasParamNames(MethodInfo method, RhspParameterCollection rhspParams)
        {
            int validNameCount = 0;
            foreach (RhspParameter rhspParam in rhspParams.ToArray())
            {
                if (methodHasParamName(method, rhspParam))
                {
                    validNameCount++;
                }
            }

            // Only true when the number of valid parameters matches the param count.
            return (validNameCount == method.GetParameters().Length);
        }

        private bool methodHasParamName(MethodInfo method, RhspParameter rhspParam)
        {
            bool hasName = false;
            foreach (ParameterInfo systemParam in method.GetParameters())
            {
                if (systemParam.Name == rhspParam.Name)
                {
                    hasName = true;
                    break;
                }
            }
            return hasName;
        }

        private ParameterInfo findSystemParam(ParameterInfo[] parameterArray, RhspParameter rhspParam)
        {
            ParameterInfo param = null;
            foreach (ParameterInfo checkParam in parameterArray)
            {
                if (checkParam.Name == rhspParam.Name)
                {
                    param = checkParam;
                    break;
                }
            }
            return param;
        }
    }
}
