namespace Chipin_Rewrite.Models.Entities;

public partial class TokenWallet
{
    public int TokenWalletId { get; set; }

    public float? Amount { get; set; }

    public string ChipinId { get; set; }

    public virtual UserTable? Chipin { get; set; } = null!;
}
