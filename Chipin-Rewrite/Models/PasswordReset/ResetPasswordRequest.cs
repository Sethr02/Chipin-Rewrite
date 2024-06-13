namespace Chipin_Rewrite.Models.PasswordReset
{
    public class ResetPasswordLinkRequest
    {
        public string ChipinId { get; set; }
        public string Email { get; set; }
    }
}
