using Chipin_Rewrite.Models.CallPay;

namespace Chipin_Rewrite.Utility.CallPay
{
    public class CallPayService : ICallPayService
    {

        public async Task<HttpResponseMessage> ValidatePayment(BankDetails bankDetails)
        {

            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            string endpoint = "https://eftsecure.callpay.com/api/v2/payout/validate-account";

            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());


                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("transaction[bank]", bankDetails.BankName),
                    new KeyValuePair<string, string>("transaction[branch]", bankDetails.BranchCode),
                    new KeyValuePair<string, string>("transaction[account]", bankDetails.AccountNumber),
                    new KeyValuePair<string, string>("transaction[customer_name]", bankDetails.CustomerName)
                });

                HttpResponseMessage response = await httpClient.PostAsync(endpoint, requestData);

                return response;
            }
        }

        public async Task<HttpResponseMessage> Payout(CallPayPayoutParams payoutParams)
        {

            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            string endpoint = "https://eftsecure.callpay.com/api/v2/payout/queue";

            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("transaction[bank]", payoutParams.BankDetails.BankName),
                    new KeyValuePair<string, string>("transaction[branch]", payoutParams.BankDetails.BranchCode),
                    new KeyValuePair<string, string>("transaction[account]", payoutParams.BankDetails.AccountNumber),
                    new KeyValuePair<string, string>("transaction[customer_name]", payoutParams.BankDetails.CustomerName),
                    new KeyValuePair<string, string>("transaction[amount]", payoutParams.Amount.ToString()),
                    new KeyValuePair<string, string>("transaction[reference]", payoutParams.Reference)
                });

                var response = await httpClient.PostAsync(endpoint, requestData);

                return response;
            }
        }
    }
}
