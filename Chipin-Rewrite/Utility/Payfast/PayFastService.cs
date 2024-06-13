using Chipin_Rewrite.Models.Payfast;
using PayFast;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Chipin_Rewrite.Utility.Payfast
{
    public class PayFastService : IPayFastService
    {

        private readonly ILogger _logger;

        public PayFastService(ILogger<PayFastService> logger)
        {
            _logger = logger;
        }

        public string GenerateSignature(Dictionary<string, string> data, string passPhrase = "")
        {
            var payload = new StringBuilder();

            foreach (var item in data)
            {
                payload.Append($"{item.Key}={HttpUtility.UrlEncode(item.Value.Replace("+", " "))}&");
            }

            payload.Length--; // Remove the last '&'

            if (!string.IsNullOrEmpty(passPhrase))
            {
                payload.Append($"&passphrase={passPhrase}");
            }

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(payload.ToString()));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<string> GetPayFastPaymentLink(PayfastPayload payfastPayload)
        {
            // Your merchant ID and merchant key
            PayFastRequest payFastRequest = new PayFastRequest("BrandHubb2020");
            payFastRequest.merchant_id = "16403756";
            payFastRequest.merchant_key = "iho3kxn48k6xw";
            payFastRequest.return_url = payfastPayload.ReturnUrl;
            payFastRequest.cancel_url = payfastPayload.CancelUrl;
            payFastRequest.notify_url = payfastPayload.NotifyUrl;

            // Buyer Details


            // Transaction Details

            payFastRequest.amount = (double)payfastPayload.Amount;
            payFastRequest.item_name = payfastPayload.ItemName;
            payFastRequest.custom_int1 = payfastPayload.CustomInt1;




            string redirectUrl = $"https://payfast.co.za/eng/process?{payFastRequest.ToString()}";
            return redirectUrl;
        }


    }
}

