namespace Chipin_Rewrite.Models.SignedModels
{
    public class ReturnChipinId
    {
        public string? ChipinId { get; set; }
        public string Cart { get; set; } = null!;
        public string Signature { get; set; } = null!;

    }
}
