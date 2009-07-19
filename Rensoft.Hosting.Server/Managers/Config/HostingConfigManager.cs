using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace Rensoft.Hosting.Server.Managers.Config
{
    [RhspManagerUsage("HostingConfig")]
    public class HostingConfigManager : ConfigManager
    {
        private const string configFile = "HostingConfig.xml";
        private const string firstNode = "HostingConfig";

        protected string ConfigPath
        {
            get { return Path.Combine(ConfigDirectory, configFile); }
        }

        public void Create(IRhspData data)
        {
            updateChildDataParent(data);
            create(data, ConfigDocument);
        }

        public void Update(IRhspData data)
        {
            updateChildDataParent(data);
            update(data, ConfigDocument);
        }

        public void Delete<TData>(RhspDataID dataID)
        {
            delete(dataID, typeof(TData));
        }

        public bool Exists<TData>(RhspDataID dataID)
            where TData : IRhspData
        {
            return Exists(dataID, typeof(TData));
        }

        public bool Exists(RhspDataID dataID, Type type)
        {
            RhspDataAttribute hda = GetRuntimeHda(type);
            XElement parent = getParent(ConfigDocument, hda.Parent);
            return find(parent, dataID, hda).Count() != 0;
        }

        private RhspDataAttribute GetRuntimeHda(Type type)
        {
            return RhspData.GetRuntimeHda(type);
        }

        public TData GetSingle<TData>(RhspDataID dataID)
            where TData : IRhspData
        {
            return (TData)getSingleOfType(dataID, typeof(TData));
        }

        public TData[] GetArray<TData>()
            where TData : IRhspData
        {
            return GetQueryForAllOfType<TData>().ToArray();
        }

        private void delete(RhspDataID dataID, Type dataType)
        {
            IRhspData data = (IRhspData)getSingleOfType(dataID, dataType);
            deleteWithoutSave(dataID, dataType, ConfigDocument);

            if (data is IRhspDataParent)
            {
                // Set each child to deleted, then process.
                IRhspDataParent dataParent = (IRhspDataParent)data;
                List<IRhspDataChild> childList = dataParent.GetDataChildren().ToList();

                // Process all the marked deletions.
                childList.ForEach(c => c.PendingAction = ChildPendingAction.Delete);
            }
        }

        protected override XDocument LoadConfigDocumentInternal()
        {
            return LoadConfigDocumentInternal(ConfigPath, firstNode);
        }

        private object getSingleOfType(RhspDataID dataID, Type dataType)
        {
            XElement element = GetElement(dataID, dataType);
            return createFromElement(element, dataType);
        }

        public IEnumerable<TData> GetQueryForAllOfType<TData>()
            where TData : IRhspData
        {
            RhspDataAttribute hdca = GetRuntimeHda(typeof(TData));
            XElement parent = getParent(ConfigDocument, hdca.Parent);

            return from element in parent.Elements()
                   select (TData)createFromElement(element, typeof(TData));
        }

        private object createFromElement(XElement element, Type dataType)
        {
            try
            {
                RhspDataAttribute hda = GetRuntimeHda(dataType);
                object data = Activator.CreateInstance(dataType);
                if (data is IRhspData)
                {
                    IRhspData hd = (IRhspData)data;
                    hd.DataID = new RhspDataID(element.Element(hda.ID).Value);
                    hd.FromElement(element, this);
                }
                else
                {
                    throw new NotSupportedException(
                        "The data type must implement interface " +
                        typeof(IRhspData).Name + " for an instance " +
                        "to be created from an XML element.");
                }
                return data;
            }
            catch (TargetInvocationException ex)
            {
                // Rethrow the target invocation exception, but pull the message to the top.
                throw new TargetInvocationException(ex.InnerException.Message, ex.InnerException);
            }
        }

        private void update(IRhspData data, XDocument document)
        {
            RhspDataAttribute hdca = GetRuntimeHda(data.GetType());
            XElement element = getElementUsingHda(document, data.DataID, hdca);

            // Delegate element update to object it's self.
            data.UpdateElement(element, this);

            /* Set the latest schema version (as re-saving automatically
             * updates the object to this version). Versions are only ever
             * opened as earlier versions and never saved as this. */
            updateVersionElement(element, hdca);
        }

        private void updateVersionElement(XElement element, RhspDataAttribute hdca)
        {
            var q = from e in element.Elements()
                    where e.Name == SchemaVersionElement
                    select e;

            /* Due to a bug in earlier versions, there may be multiple version tags,
             * so remove any version tags that exist already. */
            q.Remove();

            if (hdca.SchemaVersion != 0)
            {
                // Re-add the element after removing any traces (only if version specified).
                element.AddFirst(new XElement(SchemaVersionElement, hdca.SchemaVersion));
            }
        }

        private void deleteWithoutSave(RhspDataID dataID, Type dataType, XDocument document)
        {
            RhspDataAttribute hdca = GetRuntimeHda(dataType);
            XElement element = getElementUsingHda(document, dataID, hdca);

            // Simply remove the element.
            element.Remove();
        }

        private XElement getElementUsingHda(
            XDocument document,
            RhspDataID dataID,
            RhspDataAttribute hda)
        {
            XElement parent = getParent(document, hda.Parent);
            var uniqueQuery = find(parent, dataID, hda);

            // Ensure that ID actually exists.
            if (uniqueQuery.Count() == 0)
            {
                throw new InvalidOperationException(
                    "Unable to find element with unique ID of '" + dataID.Value + "'.");
            }

            return uniqueQuery.First();
        }

        public XElement GetElement<TData>(XDocument document, RhspDataID dataID)
        {
            return GetElement(dataID, typeof(TData));
        }

        [Obsolete("Use GetElement(HostingDataID, Type) instead.")]
        public XElement GetElement(XDocument document, RhspDataID dataID, Type dataType)
        {
            return GetElement(dataID, dataType);
        }

        public XElement GetElement(RhspDataID dataID, Type dataType)
        {
            RhspDataAttribute hda = GetRuntimeHda(dataType);
            XElement element = getElementUsingHda(ConfigDocument, dataID, hda);
            return element;
        }

        private void create(IRhspData data, XDocument document)
        {
            if (data.DataID == null)
            {
                throw new Exception(
                    "Hosting ID cannot be unassigned.");
            }

            RhspDataAttribute hda = GetRuntimeHda(data.GetType());
            XElement parent = getParent(document, hda.Parent);
            var uniqueQuery = find(parent, data.DataID, hda);

            // Ensure that IDs are not duplicated.
            if (uniqueQuery.Count() != 0)
            {
                throw new InvalidOperationException(
                    "An element with name '" + hda.Name + "' in parent " +
                    "'" + hda.Parent + "' already contains unique ID " +
                    "with value of '" + data.DataID + "'.");
            }

            // Get original data without unique ID present.
            XElement element = data.ToXElement(this);

            // Add ID first for better visual diagnosis.
            element.AddFirst(new XElement(hda.ID, data.DataID.Value));

            if (hda.SchemaVersion != 0)
            {
                // Add version before the ID element, but only if neccecary.
                element.AddFirst(new XElement(SchemaVersionElement, hda.SchemaVersion));
            }

            // Add the element with unique ID present.
            parent.Add(element);
        }

        private IEnumerable<XElement> find(
            XElement parent,
            RhspDataID dataID,
            RhspDataAttribute hda)
        {
            if (dataID == null)
            {
                throw new ArgumentNullException("dataID");
            }

            return from child in
                       (
                           from element in parent.Elements()
                           select element.Element(hda.ID)
                       )
                   where child.Value == dataID.Value
                   select child.Parent; // Select the owner of the ID.
        }

        private XElement getParent(XDocument document, string parent)
        {
            var query = from element in ((XElement)document.FirstNode).Elements()
                        where element.Name == parent
                        select element;

            XElement parentElement;
            if (query.Count() != 0)
            {
                parentElement = query.First();
            }
            else
            {
                parentElement = new XElement(parent);
                ((XElement)document.FirstNode).Add(parentElement);
            }
            return parentElement;
        }

        private void updateChildDataParent(IRhspData data)
        {
            if (data is IRhspDataParent)
            {
                IRhspDataParent parent = (IRhspDataParent)data;

                // Apply parent ID to all child data.
                parent.GetDataChildren().ToList().ForEach(
                    c =>
                    {
                        c.ParentID = data.DataID;
                        c.Parent = parent;
                    }
                );
            }
        }

        protected override void SaveConfigDocumentInternal()
        {
            ConfigDocument.Save(ConfigPath);
        }
    }
}
