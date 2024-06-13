namespace Chipin_Rewrite.Models.Entities;

public partial class Address
{
    public int AddressId { get; set; }

    public string? AdressName { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostCode { get; set; }

    public bool? IsDefault { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string ChipinId { get; set; }

    public virtual UserTable? Chipin { get; set; } = null!;

    public virtual ICollection<ProductListWallet> ProductListWallets { get; set; } = new List<ProductListWallet>();
}
