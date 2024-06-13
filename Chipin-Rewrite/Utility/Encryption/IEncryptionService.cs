namespace Chipin_Rewrite.Utility.Encryption
{
    public interface IEncryptionService
    {
        public string EncryptAES(string data, string key);
        public string DecryptAES(string encryptedData, string key);
    }
}
