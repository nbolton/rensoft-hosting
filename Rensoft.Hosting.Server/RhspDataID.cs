using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Collections;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    public class RhspDataID
    {
        [DataMember]
        public string Value { get; set; }

        public RhspDataID(string value)
        {
            this.Value = value;
        }

        public static RhspDataID Generate()
        {
            return Generate(Guid.NewGuid().ToString());
        }

        public static RhspDataID Generate(string seed)
        {
            return new RhspDataID(createHash(seed));
        }

        private static string createHash(string seed)
        {
            SHA1Managed sha1 = new SHA1Managed();
            byte[] textBuffer = Encoding.UTF8.GetBytes(seed);
            byte[] hashBuffer = sha1.ComputeHash(textBuffer);
            string hashText = BitConverter.ToString(hashBuffer);
            return hashText.Replace("-", String.Empty).ToLower();
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object o)
        {
            RhspDataID other = (RhspDataID)o;
            if ((Value != null)
                && (o != null)
                && (other.Value != null))
            {
                return Value.Equals(other.Value);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            if (Value != null)
            {
                return Value.GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        private static bool eitherAreNull(object a, object b)
        {
            return a == null || b == null;
        }

        private static bool bothAreNull(object a, object b)
        {
            return a == null && b == null;
        }

        public static bool operator ==(RhspDataID a, RhspDataID b)
        {
            if (bothAreNull(a, b))
            {
                return true;
            }
            else if (eitherAreNull(a, b))
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }

        public static bool operator !=(RhspDataID a, RhspDataID b)
        {
            if (bothAreNull(a, b))
            {
                return false;
            }
            else if (eitherAreNull(a, b))
            {
                return true;
            }
            else
            {
                return !a.Equals(b);
            }
        }
    }
}
