using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Diagnostics;
using System.Reflection;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    //[DebuggerStepThrough]
    public abstract class RhspAdapter
    {
        private RhspClientManager manager;
        private bool initialized = false;

        public event RhspAdapterErrorEventHandler ErrorOccured;

        public void Initialize(RhspClientManager manager)
        {
            this.manager = manager;
            this.initialized = true;
        }

        /// <summary>
        /// Gets the result of the response from a specified method.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="method">The method name to execute.</param>
        /// <returns>Result within the response.</returns>
        protected T Run<T>(string method)
        {
            return Run<T>(method, null);
        }

        /// <summary>
        /// Executes the request and ignores the response data.
        /// </summary>
        /// <param name="method">The method name to execute.</param>
        protected void Run(string method)
        {
            Run(method, null);
        }

        /// <summary>
        /// Gets the result of the response from a specified method with anonymous parameters.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="method">The method name to execute.</param>
        /// <param name="anonymousParams">An anonymous type object such as: new { foo, bar }</param>
        /// <returns>Result within the response.</returns>
        protected T Run<T>(string method, object anonymousParams)
        {
            return executeCommand(method, anonymousParams).GetResult<T>();
        }

        /// <summary>
        /// Executes the request  with anonymous parameters, and ignores the response data.
        /// </summary>
        /// <param name="method">The method name to execute.</param>
        /// <param name="anonymousParams">An anonymous type object such as: new { foo, bar }</param>
        protected void Run(string method, object anonymousParams)
        {
            executeCommand(method, anonymousParams);
        }

        [Obsolete("Use Run to execute commands instead.")]
        public RhspConnection CreateConnection()
        {
            enforceInitialized();
            return manager.CreateConnection();
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            enforceInitialized();
            return manager.CreateAdapter<TAdapter>();
        }

        private RhspCommandResponse executeCommand(string method, object anonymousParams)
        {
            try
            {
                enforceInitialized();
                RhspConnection connection = manager.CreateConnection();
                RhspAdapterUsageAttribute attribute = getUsageAttribute();
                string commandText = string.Format("{0}.{1}", attribute.ModuleName, method);
                RhspCommand command = new RhspCommand(connection, commandText);
                addAnonymousParamsToCommand(anonymousParams, command);
                return command.Execute();
            }
            catch (Exception ex)
            {
                RhspAdapterErrorEventArgs errorArgs = new RhspAdapterErrorEventArgs();
                errorArgs.ThrowException = true;
                errorArgs.Error = ex;

                if (ErrorOccured != null)
                {
                    ErrorOccured(this, errorArgs);
                }

                if (errorArgs.ThrowException)
                {
                    // When throwing has not ben disabled, throw error.
                    throw new Exception("Error occured while executing command.", ex);
                }
                else
                {
                    // Something must be returned, and null would be bad.
                    return RhspCommandResponse.Error;
                }
            }
        }

        private void enforceInitialized()
        {
            if (!initialized)
            {
                throw new InvalidOperationException(
                    "This adapter has not been initialized.");
            }
        }

        private static void addAnonymousParamsToCommand(object anonymousParams, RhspCommand command)
        {
            if (anonymousParams != null)
            {
                foreach (PropertyInfo p in anonymousParams.GetType().GetProperties())
                {
                    command.Parameters.Add(p.Name, p.GetValue(anonymousParams, null));
                }
            }
        }

        private RhspAdapterUsageAttribute getUsageAttribute()
        {
            object[] ca = GetType().GetCustomAttributes(typeof(RhspAdapterUsageAttribute), false);

            // Ensure that the class at the end of the hierachy implements the correct attribute.
            if (ca.Length == 0)
            {
                throw new Exception(
                    "Cannot use this method because the type '" + GetType().FullName + "' " +
                    "does not use the '" + typeof(RhspAdapterUsageAttribute).FullName + "' " +
                    "attribute.");
            }

            // Should be safe to call 0 index, as multiple usage is not allowed.
            return (RhspAdapterUsageAttribute)ca[0];
        }
    }
}
