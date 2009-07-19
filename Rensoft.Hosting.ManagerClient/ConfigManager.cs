using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Collections;
using Rensoft.Hosting.DataAccess;
using System.Windows.Forms;

namespace Rensoft.Hosting.ManagerClient
{
    public partial class ConfigManager
    {
        private const string configFile = "ManagerConfig.xml";
        private const string firstNode = "ManagerConfig";

        public static readonly ConfigManager Default = new ConfigManager();

        protected string ConfigPath
        {
            get
            {
                // Parent is common for every version, so save there instead.
                DirectoryInfo dataDirectory = new DirectoryInfo(Application.UserAppDataPath);
                return Path.Combine(dataDirectory.Parent.FullName, configFile); 
            }
        }

        public TValue[] GetArray<TValue>(string settingName)
        {
            XDocument document = GetConfigDocument();
            var query = getElement(document, settingName);

            List<TValue> valueList = new List<TValue>();
            if ((query.Count() != 0)
                && (query.First().Descendants().Count() != 0))
            {
                // Multiple child elements indicates an array.
                foreach (XElement element in query.First().Descendants())
                {
                    string stringValue = element.Value;
                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        valueList.Add(changeType<TValue>(stringValue));
                    }
                }
            }

            return valueList.ToArray();
        }

        public TValue GetSingle<TValue>(string settingName)
        {
            XDocument document = GetConfigDocument();
            var query = getElement(document, settingName);

            string stringValue = null;
            if (query.Count() != 0)
            {
                stringValue = query.First().Value;
            }

            if (!string.IsNullOrEmpty(stringValue))
            {
                // When not null, try and cast the value to the correct type.
                return changeType<TValue>(stringValue);
            }
            else
            {
                // Instead of coverting from null, just return null.
                return default(TValue);
            }
        }

        private TValue changeType<TValue>(object value)
        {
            if (typeof(TValue).IsEnum)
            {
                return (TValue)Enum.Parse(typeof(TValue), value.ToString());
            }
            else
            {
                return (TValue)Convert.ChangeType(value, typeof(TValue));
            }
        }

        public void SetArray<TValue>(string settingName, TValue[] valueArray)
        {
            XDocument document = GetConfigDocument();
            var query = getElement(document, settingName);

            if (query.Count() != 0)
            {
                // Instead of updating elements, just replace original.
                query.First().Remove();
            }

            XElement arrayElement = new XElement(settingName);
            foreach (object value in valueArray)
            {
                string stringValue = changeType<string>(value);
                if (!string.IsNullOrEmpty(stringValue))
                {
                    arrayElement.Add(new XElement("ArrayItem", stringValue));
                }
            }

            // Add the replacement array element.
            ((XElement)document.FirstNode).Add(arrayElement);

            document.Save(ConfigPath);
        }

        public void SetSinlge<TValue>(string settingName, TValue value)
        {
            XDocument document = GetConfigDocument();
            var query = getElement(document, settingName);
            string stringValue = (string)Convert.ChangeType(value, typeof(string));

            if (query.Count() != 0)
            {
                query.First().Value = stringValue;
            }
            else
            {
                ((XElement)document.FirstNode).Add(new XElement(settingName, value));
            }

            document.Save(ConfigPath);
        }

        private IEnumerable<XElement> getElement(XDocument document, string name)
        {
            return getElement(((XElement)document.FirstNode), name);
        }

        private IEnumerable<XElement> getElement(XElement parentElement, string name)
        {
            return from element in parentElement.Descendants()
                   where element.Name == name
                   select element;
        }

        protected XDocument GetConfigDocument()
        {
            XDocument document;
            if (File.Exists(ConfigPath))
            {
                document = XDocument.Load(ConfigPath);
            }
            else
            {
                document = new XDocument(new XElement(firstNode));
            }
            return document;
        }
    }
}
