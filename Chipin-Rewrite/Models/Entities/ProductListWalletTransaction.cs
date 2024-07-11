namespace Chipin_Rewrite.Models.Entities;

public partial class ProductListWalletTransaction
{
    public int ProductListWalletTransactionId { get; set; }

    public float? Amount { get; set; }

    public string ChipinId { get; set; }

    public sbyte? FromInvitedUser { get; set; }

    public int ProductListWalletId { get; set; }

    public string TransactionMethod { get; set; } = null!;
   // public string TransactionId { get; set; } = null!;
   // public string PaymentKey { get; set; } = null!;
    //public DateTime? CreatedAt { get; set; }

    public virtual ProductListWallet? ProductListWallet { get; set; } = null!;

    public virtual ICollection<ShareList> ShareLists { get; set; } = new List<ShareList>();
}
