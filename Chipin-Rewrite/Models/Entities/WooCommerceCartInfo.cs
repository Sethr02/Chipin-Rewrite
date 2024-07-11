namespace Chipin_Rewrite.Models.Entities
{
    public class WooCommerceCartInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Store { get; set; }
        public string CustString1 { get; set; }
        public int CustInt1 { get; set; }
        public int Quantity { get; set; }
    }

}
