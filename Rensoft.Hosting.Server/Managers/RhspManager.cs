using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Rensoft.Hosting.Server.Managers.Config;
using Rensoft.Hosting.Server.Data;
using System.Xml.Linq;
using System.IO;

namespace Rensoft.Hosting.Server.Managers
{
    [DebuggerStepThrough]
    public abstract class RhspManager
    {
        protected const string SchemaVersionElement = "SchemaVersion";

        private RhspServiceContext context;

        public event EventHandler Load;
        public event EventHandler BeforeInvokeMethod;
        public event EventHandler AfterInvokeMethod;

        public RhspServiceContext Context
        {
            get { return context; }
        }

        public ServerConfigManager ServerConfig
        {
            get { return context.ServerConfig; }
        }

        public HostingConfigManager HostingConfig
        {
            get { return context.HostingConfig; }
        }

        public object InvokeMethod(MethodInfo method, object[] parameters)
        {
            OnBeforeInvokeMethod(EventArgs.Empty);
            
            // Invoke in extenal class so debugger doesnt step through exception.
            RhspManagerInvoker invoker = new RhspManagerInvoker(this, method, parameters);
            object result = invoker.Invoke();

            OnAfterInvokeMethod(EventArgs.Empty);

            return result;
        }

        protected virtual void OnBeforeInvokeMethod(EventArgs e)
        {
            if (BeforeInvokeMethod != null) BeforeInvokeMethod(this, e);
        }

        protected virtual void OnAfterInvokeMethod(EventArgs e)
        {
            if (AfterInvokeMethod != null) AfterInvokeMethod(this, e);
        }

        protected virtual void OnLoad(EventArgs e)
        {
            if (Load != null) Load(this, e);
        }

        public static RhspManager CreateManager(RhspServiceContext context, Type managerType)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "Context has not yet been set.");
            }

            if (!typeof(RhspManager).IsAssignableFrom(managerType))
            {
                throw new RhspException(
                    "The requested type '" + managerType + "' is not compatable " +
                    "with manager type '" + typeof(RhspManager).Name + "'");
            }

            RhspManager manager = (RhspManager)Activator.CreateInstance(managerType);
            manager.context = context;
            manager.OnLoad(EventArgs.Empty);
            return manager;
        }

        public static TManager CreateManager<TManager>(RhspServiceContext context)
            where TManager : RhspManager
        {
            return (TManager)CreateManager(context, typeof(TManager));
        }

        public TManager CreateManager<TManager>()
            where TManager : RhspManager
        {
            return CreateManager<TManager>(context);
        }

        public Password EncryptPassword(string plainText)
        {
            return Password.FromPlainText(plainText, ServerConfig.EncryptorSecret);
        }

        public string DecryptPassword(Password password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            return password.GetDecrypted(ServerConfig.EncryptorSecret);
        }

        public int GetDataSchemaVersion<TData>(RhspDataID dataID)
            where TData : RhspData
        {
            return GetDataSchemaVersion(dataID, typeof(TData));
        }

        public int GetDataSchemaVersion(RhspDataID dataID, Type type)
        {
            int version = 0;
            if ((dataID != null) && HostingConfig.Exists(dataID, type))
            {
                XElement e = HostingConfig.GetElement(dataID, type);
                version = e.GetElementValue<int>(SchemaVersionElement, true);
            }

            if (version != 0)
            {
                return version;
            }
            else
            {
                // Return the default where none set.
                return RhspData.FirstSchemaVersion;
            }
        }

        protected void SetParent(IRhspDataParent parent, IEnumerable<IRhspDataChild> children)
        {
            children.ToList().ForEach(c => SetParent(parent, c));
        }

        protected void SetParent(IRhspDataParent parent, IRhspDataChild child)
        {
            child.Parent = parent;
            child.ParentID = parent.DataID;
        }

        public class MakeDirectoryObsoleteResult
        {
            public Exception Error;
        }

        protected MakeDirectoryObsoleteResult TryMakeDirectoryObsolete(DirectoryInfo directory)
        {
            MakeDirectoryObsoleteResult r = new MakeDirectoryObsoleteResult();
            try
            {
                MakeDirectoryObsolete(directory);
            }
            catch (Exception ex)
            {
                r.Error = ex;
            }
            return r;
        }

        protected void MakeDirectoryObsolete(DirectoryInfo directory)
        {
            try
            {
                if (directory.Exists)
                {
                    // Use tick count because its less complicated.
                    directory.MoveTo(directory + "-" + Environment.TickCount);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Could not make directory '" + directory + "' obsolete: " + ex.Message, ex);
            }
        }
    }
}
