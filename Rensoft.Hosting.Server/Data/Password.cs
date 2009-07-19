using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Rensoft.Crypto;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class Password
    {
        [DataMember]
        public byte[] Data { get; set; }

        public string GetDecrypted(string secret)
        {
            RsEncryptor encryptor = new RsEncryptor(secret);
            return encryptor.Decrypt(Data);
        }

        public string GetEncrypted()
        {
            return Convert.ToBase64String(Data);
        }

        public static Password FromPlainText(string plainText, string secret)
        {
            RsEncryptor encryptor = new RsEncryptor(secret);
            Password password = new Password();
            password.Data = encryptor.Encrypt(plainText);
            return password;
        }

        public static Password FromEncrypted(string encrypted)
        {
            Password password = new Password();
            password.Data = Convert.FromBase64String(encrypted);
            return password;
        }
    }
}
