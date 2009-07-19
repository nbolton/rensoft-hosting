using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Security.Cryptography;
using Rensoft.Hosting.Server.Managers.Config;
using Rensoft.Hosting.Server.Managers;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class FtpUser : IRhspData, IRhspDataChild
    {
        public RhspDataID ParentID { get; set; }
        public IRhspDataParent Parent { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public Password Password { get; set; }

        [DataMember]
        public ChildPendingAction PendingAction { get; set; }

        [DataMember]
        public RhspDataID DataID { get; set; }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "FtpUser",
                new XAttribute("Enabled", Enabled),
                new XElement("UserName", UserName),
                new XElement("Password", Password.GetEncrypted())
            );
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetAttributeValue("Enabled", Enabled);
            element.SetAttributeValue("Password", Password.GetEncrypted());
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            this.Enabled = element.GetAttributeValue<bool>("Enabled", false);
            this.UserName = element.GetElementValue<string>("UserName", false);
            this.Password = Password.FromEncrypted(element.GetElementValue<string>("Password", false));
        }

        public static FtpUser CreateFromElement(XElement element, HostingConfigManager manager)
        {
            FtpUser user = new FtpUser();
            user.FromElement(element, manager);
            return user;
        }

        public string GetPasswordMd5Hash(RhspManager manager)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] textBuffer = Encoding.ASCII.GetBytes(manager.DecryptPassword(Password));
            byte[] hashBuffer = md5.ComputeHash(textBuffer);
            string hashText = BitConverter.ToString(hashBuffer);
            return hashText.Replace("-", String.Empty).ToLower();
        }
    }
}
