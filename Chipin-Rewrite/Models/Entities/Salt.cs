namespace Chipin_Rewrite.Models.Entities;

public partial class Salt
{
    public int SaltId { get; set; }

    public string? ChipinId { get; set; }

    public string? Salt1 { get; set; }

    public virtual UserTable? Chipin { get; set; }
}
