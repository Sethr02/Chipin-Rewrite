using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Chipin_Rewrite.Models.Entities
{
    public class CheckoutData
    {
        public int OrderId { get; set; }
        public BillingInfo BillingInfo { get; set; }
        public List<CartItem> CartInfo { get; set; }
        public string CustomField { get; set; }
    }
}
