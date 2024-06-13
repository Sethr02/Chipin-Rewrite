namespace Chipin_Rewrite.Models.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? Store { get; set; }

    public string? ProductImage { get; set; }

    public int? Quantity { get; set; }

    public float? Price { get; set; }

    public string? ProductImage1 { get; set; }

    public string? ProductImage2 { get; set; }

    public string? ProductImage3 { get; set; }

    public virtual ICollection<ProductListItem> ProductListItems { get; set; } = new List<ProductListItem>();
}
