using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Rensoft.Hosting.Server.Data;
using System.Threading;

namespace Rensoft.Hosting.Server.Managers
{
    public class FileZillaManager : RhspManager
    {
        private const string rootElement = "FileZillaServer";
        private const string groupsElement = "Groups";
        private const string usersElement = "Users";

        public bool GroupExists(string name)
        {
            XDocument document = getConfigDocument();
            return getGroupQuery(name, document).Count() != 0;
        }

        public void CreateGroup(string name, DirectoryInfo homeDirectory)
        {
            XDocument document = getConfigDocument();

            document.Element(rootElement).Element(groupsElement).Add(
                new XElement("Group",
                    new XAttribute("Name", name),
                    new XElement("Permissions",
                        new XElement("Permission",
                            new XAttribute("Dir", homeDirectory.ToString()),
                            createOptionElement("IsHome", 1)
                        )
                    )
                )
            );

            saveConfigDocument(document);
        }

        public void CreateUser(string name, string password, bool enabled, string groupName)
        {
            XDocument document = getConfigDocument();

            document.Element(rootElement).Element(usersElement).Add(
                new XElement("User",
                    new XAttribute("Name", name),
                    createOptionElement("Pass", getPasswordHash(password)),
                    createOptionElement("Group", groupName),
                    createOptionElement("Enabled", enabled ? 1 : 0)
                )
            );

            saveConfigDocument(document);
        }

        public void UpdateUser(string name, bool enabled)
        {
            XDocument document = getConfigDocument();

            XElement userElement = getUserQuery(name, document).First();
            updateOptionElement(userElement, "Enabled", enabled ? 1 : 0);

            saveConfigDocument(document);
        }

        public void ChangePassword(string name, string password)
        {
            XDocument document = getConfigDocument();

            XElement userElement = getUserQuery(name, document).First();
            updateOptionElement(userElement, "Password", getPasswordHash(password));

            saveConfigDocument(document);
        }

        public void DeleteGroup(string name, bool deleteUsers)
        {
            XDocument document = getConfigDocument();

            var groups = getGroupQuery(name, document);

            groups.Remove();

            if (deleteUsers)
            {
                var users = from ue in document.Element(rootElement).Element(usersElement).Elements()
                            from o in ue.Elements("Option")
                            where o.Attribute("Name").Value == "Group"
                            where o.Value == name
                            select ue;

                users.Remove();
            }

            saveConfigDocument(document);
        }

        public void DeleteUser(string name)
        {
            XDocument document = getConfigDocument();
            getUserQuery(name, document).Remove();
            saveConfigDocument(document);
        }

        public void SetUserPermissions(string userName, DirectoryInfo directoryInfo, FtpPermissions permissions)
        {
            XDocument document = getConfigDocument();
            XElement permsElement = getUserQuery(userName, document).First().Element("Permissions");
            setPermissions(permsElement, directoryInfo.ToString(), permissions);
            saveConfigDocument(document);
        }

        public void SetGroupPermissions(string groupName, DirectoryInfo directoryInfo, FtpPermissions permissions)
        {
            XDocument document = getConfigDocument();
            XElement permsElement = getGroupQuery(groupName, document).First().Element("Permissions");
            setPermissions(permsElement, directoryInfo.ToString(), permissions);
            saveConfigDocument(document);
        }

        private IEnumerable<XElement> getGroupQuery(string name, XDocument document)
        {
            return from ge in document.Element(rootElement).Element(groupsElement).Elements()
                   where ge.Attribute("Name").Value == name
                   select ge;
        }

        private void updateOptionElement(XElement userElement, string name, object value)
        {
            var q = from o in userElement.Elements("Option")
                    where o.Attribute("Name").Value == name
                    select o;

            if (q.Count() != 0)
            {
                // Where exists, update.
                q.First().Value = value.ToString();
            }
            else
            {
                // Where not found, create new option.
                userElement.Add(createOptionElement(name, value));
            }
        }

        private IEnumerable<XElement> getUserQuery(string name, XDocument document)
        {
            return from ue in document.Element(rootElement).Element(usersElement).Elements()
                   where ue.Attribute("Name").Value == name
                   select ue;
        }

        private static XElement createOptionElement(string name, object value)
        {
            return new XElement("Option", new XAttribute("Name", name), value);
        }

        private string getPasswordHash(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] textBuffer = Encoding.ASCII.GetBytes(password);
            byte[] hashBuffer = md5.ComputeHash(textBuffer);
            string hashText = BitConverter.ToString(hashBuffer);
            return hashText.Replace("-", String.Empty).ToLower();
        }

        private XDocument getConfigDocument()
        {
            return XDocument.Load(getConfigFilePath());
        }

        private string getConfigFilePath()
        {
            return Path.Combine(ServerConfig.FileZillaDirectory.FullName, "FileZilla Server.xml");
        }

        private void reloadConfig()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = getServerExePath();
            startInfo.Arguments = "/reload-config";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(startInfo);
            p.WaitForExit();

            // HACK: Allow FileZilla to finish reading file.
            Thread.Sleep(200);
        }

        private string getServerExePath()
        {
            return Path.Combine(ServerConfig.FileZillaDirectory.FullName, "FileZilla server.exe");
        }

        private void saveConfigDocument(XDocument document)
        {
            document.Save(getConfigFilePath());
            reloadConfig();
        }

        private void setPermissions(XElement permsElement, string directory, FtpPermissions permissions)
        {
            var q = from e in permsElement.Elements()
                    where e.Attribute("Dir").Value == directory
                    select e;

            XElement pElement;
            if (q.Count() != 0)
            {
                pElement = q.First();
            }
            else
            {
                // Add permissions element where it doesn't exist.
                pElement = new XElement("Permission", new XAttribute("Dir", directory));
                permsElement.Add(pElement);
            }

            setOptionElementValue(pElement, "FileRead", permissions.FileRead);
            setOptionElementValue(pElement, "FileWrite", permissions.FileWrite);
            setOptionElementValue(pElement, "FileDelete", permissions.FileDelete);
            setOptionElementValue(pElement, "FileAppend", permissions.FileAppend);
            setOptionElementValue(pElement, "DirCreate", permissions.DirectoryCreate);
            setOptionElementValue(pElement, "DirDelete", permissions.DirectoryDelete);
            setOptionElementValue(pElement, "DirList", permissions.DirectoryList);
            setOptionElementValue(pElement, "DirSubdirs", permissions.DirectoryOpen);
        }

        private void setOptionElementValue(XElement pElement, string option, bool value)
        {
            setOptionElementValue(pElement, option, value ? 1 : 0);
        }

        private void setOptionElementValue(XElement pElement, string option, object value)
        {
            var q = from o in pElement.Elements("Option")
                    where o.Attribute("Name").Value == option
                    select o;

            if (q.Count() != 0)
            {
                q.First().Value = value.ToString();
            }
            else
            {
                pElement.Add(createOptionElement(option, value));
            }
        }

        public void ClearGroupPermissions(DirectoryInfo directoryInfo)
        {
            XDocument document = getConfigDocument();

            var q = from ge in document.Element(rootElement).Element("Groups").Elements()
                    from p1 in ge.Elements("Permissions")
                    from p2 in p1.Elements()
                    where p2.GetAttributeValue<string>("Dir", false) == directoryInfo.ToString()
                    select p2;

            q.ToList().ForEach(e => e.Remove());
            saveConfigDocument(document);
        }

        public void ClearUserPermissions(DirectoryInfo directoryInfo)
        {
            XDocument document = getConfigDocument();

            var q = from ue in document.Element(rootElement).Element("Users").Elements()
                    from p1 in ue.Elements("Permissions")
                    from p2 in p1.Elements()
                    where p2.GetAttributeValue<string>("Dir", false) == directoryInfo.ToString()
                    select p2;

            q.ToList().ForEach(e => e.Remove());
            saveConfigDocument(document);
        }
    }
}
