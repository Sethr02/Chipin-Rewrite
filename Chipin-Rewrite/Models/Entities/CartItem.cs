namespace Chipin_Rewrite.Models.Entities
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

}
