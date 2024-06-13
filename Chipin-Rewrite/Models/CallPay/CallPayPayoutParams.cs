namespace Chipin_Rewrite.Models.CallPay
{
    public class CallPayPayoutParams
    {
        public BankDetails BankDetails { get; set; }
        public string Reference { get; set; }
        public float Amount { get; set; }
    }
}
