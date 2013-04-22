using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YLR.YCrypto
{
    /// <summary>
    /// AES加密算法处理类。
    /// </summary>
    public class AESEncrypt
    {
        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥，支持128位和256位密钥</param>  
        /// <returns>返回加密后的密文字节数组，可以使用Convert.ToBase64String方法将字节数组转换成字符串。</returns>  
        public static byte[] encrypt(string plainText, string strKey)
        {
            //分组加密算法  
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组      
            //设置密钥及密钥向量  
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return cipherBytes;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组，可以使用Convert.FromBase64String方法从字符串获取字节数组。</param>  
        /// <param name="strKey">密钥，支持128位和256位密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static byte[] decrypt(byte[] cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return decryptBytes;
        }  
    }
}
