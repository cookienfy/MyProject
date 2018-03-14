using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyProject.Helpers
{

    public static class Base64Helper
    {
        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }


    public class CryptographyManager
    {
        private byte[] _keyByte = { };
        //Default Key
        private static string _key = "Pass1234";
        //Default initial vector
        private byte[] _ivByte = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };


        public string Encrypt(string value)
        {
            return Encrypt(value, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key)
        {
            return Encrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Encrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">plain text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Encrypt(string value, string key, string iv)
        {
            string encryptValue = string.Empty;
            MemoryStream ms = null;
            CryptoStream cs = null;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        byte[] inputByteArray =
                            Encoding.UTF8.GetBytes(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateEncryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        encryptValue = Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return encryptValue;
        }

        /// <summary> 
        /// Decrypt text 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value)
        {
            return Decrypt(value, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key">string key</param> 
        /// <returns>plain text</returns> 
        public string Decrypt(string value, string key)
        {
            return Decrypt(value, key, string.Empty);
        }

        /// <summary> 
        /// Decrypt text by key with initialization vector 
        /// </summary> 
        /// <param name="value">encrypted text</param> 
        /// <param name="key"> string key</param> 
        /// <param name="iv">initialization vector</param> 
        /// <returns>encrypted text</returns> 
        public string Decrypt(string value, string key, string iv)
        {
            string decrptValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                MemoryStream ms = null;
                CryptoStream cs = null;
                value = value.Replace(" ", "+");
                byte[] inputByteArray = new byte[value.Length];
                try
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        _keyByte = Encoding.UTF8.GetBytes
                                (key.Substring(0, 8));
                        if (!string.IsNullOrEmpty(iv))
                        {
                            _ivByte = Encoding.UTF8.GetBytes
                                (iv.Substring(0, 8));
                        }
                    }
                    else
                    {
                        _keyByte = Encoding.UTF8.GetBytes(_key);
                    }
                    using (DESCryptoServiceProvider des =
                            new DESCryptoServiceProvider())
                    {
                        inputByteArray = Convert.FromBase64String(value);
                        ms = new MemoryStream();
                        cs = new CryptoStream(ms, des.CreateDecryptor
                        (_keyByte, _ivByte), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        Encoding encoding = Encoding.UTF8;
                        decrptValue = encoding.GetString(ms.ToArray());
                    }
                }
                catch
                {
                    //TODO: write log 
                }
                finally
                {
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            return decrptValue;
        }


        public string ComputeHash(string plainText, string salt)
        {
            return ComputeHash(plainText, salt, HashName.MD5);
        }

        /// <summary> 
        /// Compute Hash 
        /// </summary> 
        /// <param name="plainText">plain text</param> 
        /// <param name="salt">salt string</param> 
        /// <param name="hashName">Hash Name</param> 
        /// <returns>string</returns> 
        public string ComputeHash(string plainText, string salt, HashName hashName)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                // Convert plain text into a byte array. 
                byte[] plainTextBytes = ASCIIEncoding.ASCII.GetBytes(plainText);
                // Allocate array, which will hold plain text and salt. 
                byte[] plainTextWithSaltBytes = null;
                byte[] saltBytes;
                if (!string.IsNullOrEmpty(salt))
                {
                    // Convert salt text into a byte array. 
                    saltBytes = ASCIIEncoding.ASCII.GetBytes(salt);
                    plainTextWithSaltBytes =
                        new byte[plainTextBytes.Length + saltBytes.Length];
                }
                else
                {
                    // Define min and max salt sizes. 
                    int minSaltSize = 4;
                    int maxSaltSize = 8;
                    // Generate a random number for the size of the salt. 
                    Random random = new Random();
                    int saltSize = random.Next(minSaltSize, maxSaltSize);
                    // Allocate a byte array, which will hold the salt. 
                    saltBytes = new byte[saltSize];
                    // Initialize a random number generator. 
                    RNGCryptoServiceProvider rngCryptoServiceProvider =
                                new RNGCryptoServiceProvider();
                    // Fill the salt with cryptographically strong byte values. 
                    rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);
                }
                // Copy plain text bytes into resulting array. 
                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }
                // Append salt bytes to the resulting array. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] =
                                        saltBytes[i];
                }
                HashAlgorithm hash = null;
                switch (hashName)
                {
                    case HashName.SHA1:
                        hash = new SHA1Managed();
                        break;
                    case HashName.SHA256:
                        hash = new SHA256Managed();
                        break;
                    case HashName.SHA384:
                        hash = new SHA384Managed();
                        break;
                    case HashName.SHA512:
                        hash = new SHA512Managed();
                        break;
                    case HashName.MD5:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }
                // Compute hash value of our plain text with appended salt. 
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                // Create array which will hold hash and original salt bytes. 
                byte[] hashWithSaltBytes =
                    new byte[hashBytes.Length + saltBytes.Length];
                // Copy hash bytes into resulting array. 
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }
                // Append salt bytes to the result. 
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
                }
                // Convert result into a base64-encoded string. 
                string hashValue = Convert.ToBase64String(hashWithSaltBytes);
                // Return the result. 
                return hashValue;
            }
            return string.Empty;
        }

        /// <summary> 
        /// Hash enum value 
        /// </summary> 
        public enum HashName
        {
            SHA1 = 1,
            MD5 = 2,
            SHA256 = 4,
            SHA384 = 8,
            SHA512 = 16
        }
    }
}