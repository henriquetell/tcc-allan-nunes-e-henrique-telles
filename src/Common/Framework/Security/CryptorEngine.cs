using Framework.Extenders;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Security
{
    public class CryptorEngine
    {
        private static string _key = "{638F404B-E841-482B-8A2C-25662832A2D5}";
        public static string Encrypt(string toEncrypt, string cryptKey = null, bool useHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            if (!string.IsNullOrWhiteSpace(cryptKey))
                _key = cryptKey;

            if (useHashing)
            {
                using (var hashmd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_key));
                    hashmd5.Clear();
                }
            }
            else
                keyArray = Encoding.UTF8.GetBytes(_key);

            using (var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                var cTransform = tdes.CreateEncryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string Decrypt(string cipherString, string cryptKey = null, bool useHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(cipherString);

            if (!string.IsNullOrWhiteSpace(cryptKey))
                _key = cryptKey;

            if (useHashing)
            {
                using (var hashmd5 = new MD5CryptoServiceProvider())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(_key));
                    hashmd5.Clear();
                }
            }
            else
                keyArray = Encoding.UTF8.GetBytes(_key);

            using (var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                var cTransform = tdes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return Encoding.UTF8.GetString(resultArray);
            }
        }

        public static RSA CreateRSA(string chavePrivada)
        {
            var rsa = RSA.Create();
            rsa.ConvertFromXmlString(chavePrivada);
            return rsa;
        }

        public static string Decrypt(RSA rsa, string data)
        {
            var dataByte = Convert.FromBase64String(data);
            var decryptedByte = rsa.Decrypt(dataByte, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedByte);
        }
    }
}
