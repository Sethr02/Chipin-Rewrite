using Chipin_Rewrite.Models.Entities;

namespace Chipin_Rewrite.Models.SignedModels
{
    public class ExternalProductInformation
    {
        public string? ChipinId { get; set; }
        public List<ExternalProduct> Products { get; set; } = new List<ExternalProduct>();

        public string Signature { get; set; } = null!;
    }
}
