namespace Chipin_Rewrite.Models.Payfast
{
    public class PayfastPayload
    {
        public decimal Amount { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int OrderId { get; set; }
        public int? CustomInt1 { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string NotifyUrl { get; set; }
    }
}
