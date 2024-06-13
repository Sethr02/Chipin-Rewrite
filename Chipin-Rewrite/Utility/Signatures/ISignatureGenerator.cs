using Newtonsoft.Json.Linq;

namespace Chipin_Rewrite.Utility.Signatures
{
    public interface ISignatureGenerator
    {
        //All functions in this interface are implemented in SignatureGenerator.cs
        public string CreateDataString(JObject jsonObject);
        public string CalculateMD5Hash(string input);
        public bool validateSignature(string data, string signature);

    }
}
