namespace Chipin_Rewrite.Models.Entities;

public partial class ProductListItem
{
    public int ChipinProductListEntryId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int ProductListWalletId { get; set; }

    public int? ExternalProductId { get; set; }

    public virtual ExternalProduct? ExternalProduct { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ProductListWallet? ProductListWallet { get; set; } = null!;
}
