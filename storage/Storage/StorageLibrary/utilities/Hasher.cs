using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace StorageLibrary.Utilities
{
    public class Hasher
    {
        public static Guid Hash(string s)
        {
            return new Guid(MD5.Create().ComputeHash(Encoding.Default.GetBytes(s)));
        }
    }
}
