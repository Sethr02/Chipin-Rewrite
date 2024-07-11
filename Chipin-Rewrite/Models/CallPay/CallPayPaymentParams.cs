namespace CallPayTest.Models
{
    public class CallPayPaymentParams
    {
        public float Amount { get; set; }
        public string MerchantReference { get; } = "Chipin";
        public string PaymentMethod { get; set; } = "fivewest";
        public string? Token { get; set; } = string.Empty;
    }
}
