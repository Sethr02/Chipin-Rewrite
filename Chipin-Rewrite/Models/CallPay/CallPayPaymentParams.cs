namespace CallPayTest.Models
{
    public class CallPayPaymentParams
    {
        public float Amount { get; set; }
        public string MerchantReference { get; } = "Chipin";
        public string? Token { get; set; } = string.Empty;

    }
}
