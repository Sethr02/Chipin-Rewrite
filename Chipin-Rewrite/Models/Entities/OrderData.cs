namespace Chipin_Rewrite.Models.Entities
{
    public class OrderData
    {
        public int OrderId { get; set; }
        public decimal CartTotal { get; set; }
        public BillingInfo BillingInfo { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
