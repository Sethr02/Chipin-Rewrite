namespace Chipin_Rewrite.Models.Entities;

public partial class ShareList
{
    public int ShareId { get; set; }

    public string? ShareName { get; set; }

    public string? ShareEmail { get; set; }

    public string? ShareMessage { get; set; }

    public DateTime? ShareDate { get; set; }

    public int ProductListWalletTransactionId { get; set; }

    public virtual ProductListWalletTransaction? ProductListWalletTransaction { get; set; } = null!;
}
