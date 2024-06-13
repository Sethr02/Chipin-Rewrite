namespace Chipin_Rewrite.Models.PasswordReset
{
    public class ResetPassword
    {
        public int ChipinId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}
