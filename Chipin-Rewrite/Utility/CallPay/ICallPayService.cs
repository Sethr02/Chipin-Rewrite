using Chipin_Rewrite.Models.CallPay;

namespace Chipin_Rewrite.Utility.CallPay
{
    public interface ICallPayService
    {
        public Task<HttpResponseMessage> ValidatePayment(BankDetails bankDetails);
        public Task<HttpResponseMessage> Payout(CallPayPayoutParams payoutParams);
    }
}
