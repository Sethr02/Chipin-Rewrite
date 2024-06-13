using Newtonsoft.Json.Linq;

namespace Chipin_Rewrite.Utility.Signatures
{
    public class SignatureGenerator : ISignatureGenerator
    {
        public string CreateDataString(JObject jsonObject)
        {
            string dataString = "";

            foreach (var property in jsonObject.Properties())
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    JArray array = (JArray)property.Value;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject item = (JObject)array[i];
                        foreach (var itemProperty in item.Properties())
                        {
                            dataString += $"{property.Name}[{i}].{itemProperty.Name}={itemProperty.Value}\n";
                        }
                    }
                }
                else if (property.Name != "Signature")
                {
                    dataString += $"{property.Name}={property.Value}\n";
                }

            }

            return dataString;
        }

        public string CalculateMD5Hash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool validateSignature(string data, string signature)
        {
            JObject jsonObject = JObject.Parse(data);
            string dataString = CreateDataString(jsonObject);
            Console.WriteLine(dataString);
            string hash = CalculateMD5Hash(dataString);
            Console.WriteLine(hash);
            return hash == signature;
        }
    }
}
