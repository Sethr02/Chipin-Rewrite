namespace Chipin_Rewrite.Models.CallPay
{
    public class CallPayAuthParams
    {
        public int OrgId { get; set; }
        public long Timestamp { get; set; }

        public string Token { get; set; } = String.Empty;
    }
}
