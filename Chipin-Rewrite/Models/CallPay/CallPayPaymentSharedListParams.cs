namespace CallPayTest.Models
{
    public class CallPayPaymentSharedListParams
    {
        public float Amount { get; set; }
        public string MerchantReference { get; } = "Chipin";
        public string PaymentMethod { get; set; } = "fivewest";
        public string ProductListWalletId { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
    }
}
