using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using Microsoft.Win32;
using Rensoft.Platform;

namespace Rensoft.Hosting.Server.Managers.Config
{
    [RhspManagerUsage("ServerConfig")]
    public partial class ServerConfigManager : ConfigManager
    {
        private const string configFile = "ServerConfig.xml";
        private const string firstNode = "ServerConfig";

        protected string ConfigPath
        {
            get { return Path.Combine(ConfigDirectory, configFile); }
        }

        protected override XDocument LoadConfigDocumentInternal()
        {
            return LoadConfigDocumentInternal(ConfigPath, firstNode);
        }

        [RhspModuleMethod]
        public object Get(string typeName, string settingName)
        {
            string stringValue = getStringValue(settingName);

            if (string.IsNullOrEmpty(stringValue))
            {
                throw new InvalidOperationException(
                    "Setting with name '" + settingName + "' does not exist.");
            }
            else
            {
                // When not null, try and cast the value to the correct type.
                return Convert.ChangeType(stringValue, Type.GetType(typeName));
            }
        }

        [RhspModuleMethod]
        public object Get(string typeName, string settingName, object defaultValue)
        {
            string stringValue = getStringValue(settingName);

            if (string.IsNullOrEmpty(stringValue))
            {
                // Return default value instead or throwing error.
                return defaultValue;
            }
            else
            {
                // When not null, try and cast the value to the correct type.
                return Convert.ChangeType(stringValue, Type.GetType(typeName));
            }
        }

        private string getStringValue(string settingName)
        {
            var query = getSettingElement(ConfigDocument, settingName);

            string stringValue = null;
            if (query.Count() != 0)
            {
                stringValue = query.First().Value;
            }
            return stringValue;
        }

        public TValue Get<TValue>(string settingName)
        {
            return (TValue)Get(typeof(TValue).FullName, settingName);
        }

        public TValue Get<TValue>(string settingName, TValue defaultValue)
        {
            return (TValue)Get(typeof(TValue).FullName, settingName, defaultValue);
        }

        [RhspModuleMethod]
        public void Set(string settingName, object value)
        {
            var query = getSettingElement(ConfigDocument, settingName);
            string stringValue = (string)Convert.ChangeType(value, typeof(string));

            if (query.Count() != 0)
            {
                query.First().Value = stringValue;
            }
            else
            {
                addSettingElement(ConfigDocument, settingName, stringValue);
            }
        }

        private void addSettingElement(XDocument document, string settingName, object value)
        {
            ((XElement)document.FirstNode).Add(new XElement(settingName, value));
        }

        private IEnumerable<XElement> getSettingElement(XDocument document, string settingName)
        {
            return from element in ((XElement)document.FirstNode).Elements()
                   where element.Name == settingName
                   select element;
        }

        protected RegistryKey Get32BitSoftwareRegistryKey()
        {
            string key;
            switch (PlatformInfo.CurrentPlatformType)
            {
                case PlatformType.X86:
                    key = @"Software";
                    break;

                case PlatformType.X64:
                    key = @"Software\Wow6432Node";
                    break;

                default:
                    throw new NotSupportedException(
                        "The current platform is neither 32-bit or 64-bit.");
            }

            return Registry.LocalMachine.OpenSubKey(key, true);
        }

        protected override void SaveConfigDocumentInternal()
        {
            ConfigDocument.Save(ConfigPath);
        }
    }
}
