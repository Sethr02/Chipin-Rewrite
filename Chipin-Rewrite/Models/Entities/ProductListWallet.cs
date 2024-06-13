namespace Chipin_Rewrite.Models.Entities;

public partial class ProductListWallet
{
    public int ProductListWalletId { get; set; }

    public float? Total { get; set; }

    public float? Funded { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? EndAt { get; set; }

    public string ChipinId { get; set; }

    public int? AddressId { get; set; }
    public int? Closed { get; set; }

    public virtual Address? Address { get; set; }

    public virtual UserTable? Chipin { get; set; } = null!;

    public virtual ICollection<ProductListItem> ProductListItems { get; set; } = new List<ProductListItem>();

    public virtual ICollection<ProductListWalletTransaction> ProductListWalletTransactions { get; set; } = new List<ProductListWalletTransaction>();
}
