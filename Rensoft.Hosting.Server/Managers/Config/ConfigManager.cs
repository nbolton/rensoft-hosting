using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Rensoft.Hosting.Server.Managers.Config
{
    public abstract class ConfigManager : RhspManager
    {
        protected XDocument ConfigDocument { get; private set; }

        public string ConfigDirectory
        {
            get { return Context.ConfigDirectory; }
        }

        public ConfigManager()
        {
            Load += new EventHandler(ConfigManager_Load);
        }

        void ConfigManager_Load(object sender, EventArgs e)
        {
            ConfigDocument = LoadConfigDocumentInternal();
        }

        protected abstract XDocument LoadConfigDocumentInternal();
        protected abstract void SaveConfigDocumentInternal();

        protected XDocument LoadConfigDocumentInternal(string filePath, string firstNode)
        {
            XDocument document;
            if (File.Exists(filePath))
            {
                document = XDocument.Load(filePath);
            }
            else
            {
                document = new XDocument(new XElement(firstNode));
            }
            return document;
        }

        public void Save()
        {
            SaveConfigDocumentInternal();
        }
    }
}
