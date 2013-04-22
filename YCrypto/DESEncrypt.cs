using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YLR.YCrypto
{
    /// <summary>
    /// DES加密算法处理类。
    /// </summary>
    public class DESEncrypt
    {
        /// <summary>  
        /// DES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥，支持8位密钥</param>  
        /// <returns>返回加密后的密文字节数组，可以使用Convert.ToBase64String方法将字节数组转换成字符串。</returns>
        public static byte[] encrypt(string plainText, string strKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(strKey);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }

        /// <summary>  
        /// DES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组，可以使用Convert.FromBase64String方法从字符串获取字节数组。</param>  
        /// <param name="strKey">密钥，支持8位</param>  
        /// <returns>返回解密后的字符串</returns> 
        public static byte[] decrypt(byte[] cipherText, string strKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(strKey);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = cipherText;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return mStream.ToArray();
        }
    }
}
