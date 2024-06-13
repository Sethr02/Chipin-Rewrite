using Chipin_Rewrite.Models.Payfast;

namespace Chipin_Rewrite.Utility.Payfast
{
    public interface IPayFastService
    {
        public Task<string> GetPayFastPaymentLink(PayfastPayload payfastPayload);
        public string GenerateSignature(Dictionary<string, string> data, string passPhrase = "");


    }
}
