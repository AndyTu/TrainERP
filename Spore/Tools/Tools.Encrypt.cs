using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore
{
    public partial class Tools
    {
        public string MD5(string input)
        {

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bt = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            string strMD5 = BitConverter.ToString(bt);
            return strMD5;

        }
    }
}
