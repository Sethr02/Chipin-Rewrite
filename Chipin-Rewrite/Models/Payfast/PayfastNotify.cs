namespace Chipin_Rewrite.Models.Payfast
{
    public class PayfastNotify
    {
        public int MPaymentId { get; set; }
        public int PfPaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal AmountGross { get; set; }
        public decimal AmountFee { get; set; }
        public decimal AmountNet { get; set; }
        public string? CustomStr1 { get; set; }
        public string? CustomStr2 { get; set; }
        public string? CustomStr3 { get; set; }
        public string? CustomStr4 { get; set; }
        public string? CustomStr5 { get; set; }
        public int? CustomInt1 { get; set; }
        public int? CustomInt2 { get; set; }
        public int? CustomInt3 { get; set; }
        public int? CustomInt4 { get; set; }
        public int? CustomInt5 { get; set; }
        public string? NameFirst { get; set; }
        public string? NameLast { get; set; }
        public string? EmailAddress { get; set; }
        public string? MerchantId { get; set; }
        public string? Signature { get; set; }
    }
}
