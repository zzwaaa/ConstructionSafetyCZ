using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MCUtil.Security
{
    /// <summary>
    /// 安全管理
    /// </summary>
    public static class SecurityManage
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">加密内容</param>
        /// <param name="key">16位</param>
        /// <param name="iv">16位</param>
        /// <returns>返回AES+base64加密</returns>
        public static string EncryptString_Aes(string plainText, string key, string iv)
        {
            //string datetime = DateTime.Now.ToString();
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            byte[] encrypted;

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(plainText);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //写进流
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }



        /// <summary>
        /// 使用AES解密字符串
        /// </summary>
        /// <param name="cipherText">使用AES+base64加密的字符串</param>
        /// <param name="key">16位</param>
        /// <param name="iv">16位</param>
        /// <returns>解密的字符串</returns>
        public static string DecryptString_Aes(string cipherText, string key, string iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            string plaintext = null;
            byte[] toDecrptyArray = Convert.FromBase64String(cipherText);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(toDecrptyArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }


        /// <summary>
        /// 字符串转换MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToMD5(string str)
        {
            string newStr = "";
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                newStr = strResult.Replace("-", "").ToUpper();
            }
            return newStr;
        }

        public static string GuidUpper()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        public static string GuidLower()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        /// <summary>
        /// byte数组转换MD5
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToMD5(byte[] bytes)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("BytesToMD5() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// RSA产生密钥
        /// </summary>
        /// <param name="xmlKeys">私钥</param>
        /// <param name="xmlPublicKey">公钥</param>
        public static void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA非对称加密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string xmlPublicKey, string content)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UnicodeEncoding()).GetBytes(content);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA加密数组
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="EncryptString">被加密的数组</param>
        /// <returns></returns>
        public static string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            try
            {
                byte[] CypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="decryptString">待解密的文本</param>
        /// <returns></returns>
        public static string RSADecryptToString(string xmlPrivateKey, string decryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(decryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// RAS解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="DecryptString">待解密的文本</param>
        /// <returns>解密成byte[]</returns>
        public static byte[] RSADecryptToBytes(string xmlPrivateKey, string DecryptString)
        {
            try
            {
                byte[] DypherTextBArray = Convert.FromBase64String(DecryptString);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                return rsa.Decrypt(DypherTextBArray, false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取hash描述表
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="HashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(string strSource, ref byte[] HashData)
        {
            try
            {
                byte[] Buffer;
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                Buffer = Encoding.UTF8.GetBytes(strSource);
                HashData = MD5.ComputeHash(Buffer);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取Hash描述表 
        /// </summary>
        /// <param name="strSource">待签名的字符串</param>
        /// <param name="strHashData">Hash描述</param>
        /// <returns></returns>
        public static bool GetHash(string strSource, ref string strHashData)
        {
            try
            {
                //从字符串中取得Hash描述   
                byte[] Buffer;
                byte[] HashData;
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                Buffer = Encoding.UTF8.GetBytes(strSource);
                HashData = MD5.ComputeHash(Buffer);
                strHashData = Convert.ToBase64String(HashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取hash描述
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="HashData">hash描述</param>
        /// <returns></returns>
        public static bool GetHash(FileStream objFile, ref byte[] HashData)
        {
            try
            {
                //从文件中取得Hash描述   
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取hash描述
        /// </summary>
        /// <param name="objFile">待签名的文件</param>
        /// <param name="strHashData">hash描述</param>
        /// <returns></returns>
        public static bool GetHash(FileStream objFile, ref string strHashData)
        {
            try
            {
                //从文件中取得Hash描述   
                byte[] HashData;
                HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();
                strHashData = Convert.ToBase64String(HashData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="strKeyPrivate">私钥</param>
        /// <param name="HashbyteSignature">待签名的hash描述</param>
        /// <param name="EncryptedSignatureData">签名后的结果</param>
        /// <returns></returns>
        public static bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.FromXmlString(strKeyPrivate);
                RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5   
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名   
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA签名  
        /// </summary>  
        /// <param name="strKeyPrivate">私钥</param>  
        /// <param name="HashbyteSignature">待签名Hash描述</param>  
        /// <param name="m_strEncryptedSignatureData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureFormatter(string strKeyPrivate, byte[] HashbyteSignature, ref string strEncryptedSignatureData)
        {
            try
            {
                byte[] EncryptedSignatureData;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPrivate);
                RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5   
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名   
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA签名  
        /// </summary>  
        /// <param name="strKeyPrivate">私钥</param>  
        /// <param name="strHashbyteSignature">待签名Hash描述</param>  
        /// <param name="EncryptedSignatureData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;

                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();


                RSA.FromXmlString(strKeyPrivate);
                RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5   
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名   
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA签名  
        /// </summary>  
        /// <param name="strKeyPrivate">私钥</param>  
        /// <param name="strHashbyteSignature">待签名Hash描述</param>  
        /// <param name="strEncryptedSignatureData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureFormatter(string strKeyPrivate, string strHashbyteSignature, ref string strEncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;
                byte[] EncryptedSignatureData;
                HashbyteSignature = Convert.FromBase64String(strHashbyteSignature);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPrivate);
                RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
                //设置签名的算法为MD5   
                RSAFormatter.SetHashAlgorithm("MD5");
                //执行签名   
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
                strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA签名验证  
        /// </summary>  
        /// <param name="strKeyPublic">公钥</param>  
        /// <param name="HashbyteDeformatter">Hash描述</param>  
        /// <param name="DeformatterData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5   
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// RSA签名验证  
        /// </summary>  
        /// <param name="strKeyPublic">公钥</param>  
        /// <param name="strHashbyteDeformatter">Hash描述</param>  
        /// <param name="DeformatterData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                byte[] HashbyteDeformatter;
                HashbyteDeformatter = Convert.FromBase64String(strHashbyteDeformatter);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5   
                RSADeformatter.SetHashAlgorithm("MD5");
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>  
        /// RSA签名验证  
        /// </summary>  
        /// <param name="strKeyPublic">公钥</param>  
        /// <param name="HashbyteDeformatter">Hash描述</param>  
        /// <param name="strDeformatterData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureDeformatter(string strKeyPublic, byte[] HashbyteDeformatter, string strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5   
                RSADeformatter.SetHashAlgorithm("MD5");
                DeformatterData = Convert.FromBase64String(strDeformatterData);
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>  
        /// RSA签名验证  
        /// </summary>  
        /// <param name="strKeyPublic">公钥</param>  
        /// <param name="strHashbyteDeformatter">Hash描述</param>  
        /// <param name="strDeformatterData">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureDeformatter(string strKeyPublic, string strHashbyteDeformatter, string strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;
                byte[] HashbyteDeformatter;
                HashbyteDeformatter = Convert.FromBase64String(strHashbyteDeformatter);
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(strKeyPublic);
                RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(RSA);
                //指定解密的时候HASH算法为MD5   
                RSADeformatter.SetHashAlgorithm("MD5");
                DeformatterData = Convert.FromBase64String(strDeformatterData);
                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static readonly string key = "NJZHGDNJZHGD";
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        /// <summary>
        /// AES128 CMAC加密算法
        /// </summary>
        /// <param name="appAuthKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string AES128CMAC(byte[] key, byte[] data)
        {
            // SubKey generation
            // step 1, AES-128 with key K is applied to an all-zero input block.
            byte[] L = AESEncrypt(key, new byte[16], new byte[16]);

            // step 2, K1 is derived through the following operation:
            byte[] FirstSubkey = Rol(L); //If the most significant bit of L is equal to 0, K1 is the left-shift of L by 1 bit.
            if ((L[0] & 0x80) == 0x80)
                FirstSubkey[15] ^= 0x87; // Otherwise, K1 is the exclusive-OR of const_Rb and the left-shift of L by 1 bit.

            // step 3, K2 is derived through the following operation:
            byte[] SecondSubkey = Rol(FirstSubkey); // If the most significant bit of K1 is equal to 0, K2 is the left-shift of K1 by 1 bit.
            if ((FirstSubkey[0] & 0x80) == 0x80)
                SecondSubkey[15] ^= 0x87; // Otherwise, K2 is the exclusive-OR of const_Rb and the left-shift of K1 by 1 bit.

            // MAC computing
            if (((data.Length != 0) && (data.Length % 16 == 0)) == true)
            {
                // If the size of the input message block is equal to a positive multiple of the block size (namely, 128 bits),
                // the last block shall be exclusive-OR'ed with K1 before processing
                for (int j = 0; j < FirstSubkey.Length; j++)
                    data[data.Length - 16 + j] ^= FirstSubkey[j];
            }
            else
            {
                // Otherwise, the last block shall be padded with 10^i
                byte[] padding = new byte[16 - data.Length % 16];
                padding[0] = 0x80;

                data = data.Concat<byte>(padding.AsEnumerable()).ToArray();

                // and exclusive-OR'ed with K2
                for (int j = 0; j < SecondSubkey.Length; j++)
                    data[data.Length - 16 + j] ^= SecondSubkey[j];
            }

            // The result of the previous process will be the input of the last encryption.
            byte[] encResult = AESEncrypt(key, new byte[16], data);

            byte[] HashValue = new byte[16];
            Array.Copy(encResult, encResult.Length - HashValue.Length, HashValue, 0, HashValue.Length);

            return BytesToHexString(HashValue);
        }


        static byte[] AESEncrypt(byte[] key, byte[] iv, byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }

        static byte[] Rol(byte[] b)
        {
            byte[] r = new byte[b.Length];
            byte carry = 0;

            for (int i = b.Length - 1; i >= 0; i--)
            {
                ushort u = (ushort)(b[i] << 1);
                r[i] = (byte)((u & 0xff) + carry);
                carry = (byte)((u & 0xff00) >> 8);
            }

            return r;
        }


        public static string BytesToHexString(byte[] bArr)
        {
            StringBuilder sb = new StringBuilder(bArr.Length);
            String sTmp;

            for (int i = 0; i < bArr.Length; i++)
            {
                sTmp = bArr[i].ToString("X2");
                sb.Append(sTmp);
            }

            return sb.ToString();
        }

        public static string ReBytesToHexString(byte[] outBytes)
        {
            StringBuilder sb = new StringBuilder(outBytes.Length);
            String sTmp;

            for (int i = outBytes.Length - 1; i >= 0; i--)
            {
                sTmp = outBytes[i].ToString("X2");
                sb.Append(sTmp);
            }

            return sb.ToString();
        }
    }

}
