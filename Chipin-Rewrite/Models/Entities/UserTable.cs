namespace Chipin_Rewrite.Models.Entities;

public partial class UserTable
{
    public string ChipinId { get; set; }

    public string? ChipinName { get; set; }

    public DateTime? ChipinCreatedDate { get; set; }

    public int? TokenWalletId { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPass { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<BillingAddress> BillingAddresses { get; set; } = new List<BillingAddress>();

    public virtual ICollection<ProductListWallet> ProductListWallets { get; set; } = new List<ProductListWallet>();
    // public virtual ICollection<TempConnection> TempConnections { get; set; } = new List<TempConnection>();

    public virtual ICollection<Salt> Salts { get; set; } = new List<Salt>();

    public virtual ICollection<TokenWallet> TokenWallets { get; set; } = new List<TokenWallet>();

    public static implicit operator List<object>(UserTable? v)
    {
        throw new NotImplementedException();
    }
}
