using Chipin_Rewrite.Models.Entities;

namespace Chipin_Rewrite.Models.SignedModels
{
    public class ReturnModel
    {
        public Address ShippingAddress { get; set; }
        public List<ExternalProduct> Products { get; set; }
        public BillingAddress BillingAddress { get; set; }

        public string? Signature { get; set; }
    }
}
