using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace YLR.YCrypto
{
    /// <summary>
    /// MD5加密算法。
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public MD5Encrypt()
        { 
        }

        /// <summary>
        /// 给二进制数据加密。
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>加密后的字符串。</returns>
        public string GetMD5(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 给字符串加密。
        /// </summary>
        /// <param name="data">要加密的字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public string GetMD5(string data)
        {
            return GetMD5(ASCIIEncoding.Default.GetBytes(data));
        }
    }
}
