using Chipin_Rewrite.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Chipin_Rewrite.Models.woocommerce
{
    public partial class TempConnection
    {
        [Key]
        public int? TempConnectionId { get; set; }
        [Required]
        public string? ReturnUrl { get; set; }
        public int? ChipinId { get; set; }
        [Required]
        public string? SessionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual UserTable? Chipin { get; set; } = null!;
    }
}
