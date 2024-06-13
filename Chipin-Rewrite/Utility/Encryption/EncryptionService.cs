using System.Security.Cryptography;

namespace Chipin_Rewrite.Utility.Encryption
{
    public class EncryptionService : IEncryptionService
    {
        //EncryptAES takes in a string and a key and returns an encrypted string using AES encryption
        public string EncryptAES(string data, string key)
        {
            byte[] encryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.GenerateIV();
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);

                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }

                    encryptedBytes = msEncrypt.ToArray();
                }
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        //DecryptAES decrypts an encrypted string using AES encryption taking in the encrypted string and the key
        public string DecryptAES(string encryptedData, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] decryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.IV = encryptedBytes.Take(16).ToArray(); // Extract the IV from the encrypted data.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new System.IO.MemoryStream())
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedBytes, 16, encryptedBytes.Length - 16); // Skip the IV during decryption.
                        csDecrypt.FlushFinalBlock();
                    }

                    decryptedBytes = msDecrypt.ToArray();
                }
            }

            return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
