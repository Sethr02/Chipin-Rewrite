namespace Chipin_Rewrite.Models.Entities;

public partial class ExternalProduct
{
    public int ExternalProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public float? Price { get; set; }

    public string? PageUrl { get; set; }

    public string? Store { get; set; }

    public string? CustString1 { get; set; }

    public string? CustString2 { get; set; }

    public string? CustString3 { get; set; }

    public int? CustInt1 { get; set; }

    public int? CustInt2 { get; set; }

    public int? CustInt3 { get; set; }

    public string? Image { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string? ReturnUrl { get; set; }
    public int? Quantity { get; set; }
    public string ProductId { get; set; } = null!;

    public virtual ICollection<ProductListItem> ProductListItems { get; set; } = new List<ProductListItem>();
}
