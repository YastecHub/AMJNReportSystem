using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Persistence.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        private const string keyValue = "E546C8DF278CD5931069B522E695D4F2";

        public EncryptionService() { }
        public string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            try
            {
                var key = Encoding.UTF8.GetBytes(keyValue);

                using var aesAlg = Aes.Create();
                using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
                using var msEncrypt = new MemoryStream();
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(value);
                }

                var iv = aesAlg.IV;

                var decryptedContent = msEncrypt.ToArray();

                var result = new byte[iv.Length + decryptedContent.Length];

                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                var str = Convert.ToBase64String(result);
                var fullCipher = Convert.FromBase64String(str);
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            try
            {
                encryptedText = encryptedText.Replace(" ", "+");
                var fullCipher = Convert.FromBase64String(encryptedText);

                var iv = new byte[16];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
                var key = Encoding.UTF8.GetBytes(keyValue);

                using var aesAlg = Aes.Create();
                using var decryptor = aesAlg.CreateDecryptor(key, iv);
                string result;
                using (var msDecrypt = new MemoryStream(cipher))
                {
                    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using var srDecrypt = new StreamReader(csDecrypt);
                    result = srDecrypt.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
