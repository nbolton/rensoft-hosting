using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Crypto;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class Password
    {
        private Password() { }

        public void SetEncrypted(string plainText, string secret)
        {
            RsEncryptor encryptor = new RsEncryptor(secret);
            Data = encryptor.Encrypt(plainText);
        }

        public string GetDecrypted(string secret)
        {
            RsEncryptor encryptor = new RsEncryptor(secret);
            return encryptor.Decrypt(Data);
        }

        public static Password FromPlainText(string plainText, string secret)
        {
            Password password = new Password();
            password.SetEncrypted(plainText, secret);
            return password;
        }
    }
}
