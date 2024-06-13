using Chipin_Rewrite.Models.CallPay;
using System.Security.Cryptography;
using System.Text;

namespace Chipin_Rewrite.Utility.CallPay
{
    public class CallPayAuthTokenGenerator
    {

        public CallPayAuthTokenGenerator()
        {

        }
        public CallPayAuthParams GenerateToken()
        {
            string salt = "xd7DkCyuRXQNtARtE7_J_NVU";
            int orgId = 18122;
            long timestamp = DateTime.UtcNow.Ticks;

            CallPayAuthParams authParams = new CallPayAuthParams()
            {
                OrgId = orgId,
                Timestamp = timestamp
            };

            string toHash = $"{salt}_{orgId}_{timestamp}";
            string hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));
                hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                Console.WriteLine("Hash: " + hash);
            }

            authParams.Token = hash;

            return authParams;
        }

    }


}
