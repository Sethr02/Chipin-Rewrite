namespace Chipin_Rewrite.Models.CallPay
{
    using System;

    public class PaymentTransaction
    {
        public string? CallpayTransactionId { get; set; }
        public int Success { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public string? Reason { get; set; }
        public string? User { get; set; }
        public string? MerchantReference { get; set; }
        public string? GatewayReference { get; set; }
        public int OrganisationId { get; set; }
        public GatewayResponse GatewayResponse { get; set; }
        public int IsDemoTransaction { get; set; }
        public string? Currency { get; set; }
        public string? PaymentKey { get; set; }
    }

    public class GatewayResponse
    {
        public string? Id { get; set; }
        public string? RegistrationId { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentBrand { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Descriptor { get; set; }
        public string? MerchantTransactionId { get; set; }
        public Result Result { get; set; }
        public ResultDetails ResultDetails { get; set; }
        public Card Card { get; set; }
        public Customer Customer { get; set; }
        public CustomParameters CustomParameters { get; set; }
        public Risk Risk { get; set; }
        public string? BuildNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Ndc { get; set; }
        public StandingInstruction StandingInstruction { get; set; }
        public bool Successful { get; set; }
        public string? TokenExpiry { get; set; }
        public string? UserToken { get; set; }
        public string? TokenGuid { get; set; }
        public string? Status { get; set; }
        public string? Bin { get; set; }
        public string? CardToken { get; set; }
    }

    public class Result
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
    }

    public class ResultDetails
    {
        public string? ExtendedDescription { get; set; }
        public string? AuthCode { get; set; }
        public string? ConnectorTxID1 { get; set; }
        public string? ConnectorTxID3 { get; set; }
        public string? ConnectorTxID2 { get; set; }
        public string? AcquirerResponse { get; set; }
        public string? ReconciliationId { get; set; }
        public string? CardholderInitiatedTransactionID { get; set; }
    }

    public class Card
    {
        public string? Bin { get; set; }
        public string? Last4Digits { get; set; }
        public string? Holder { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
    }

    public class Customer
    {
        public string? Ip { get; set; }
        public Browser Browser { get; set; }
    }

    public class Browser
    {
        public string? AcceptHeader { get; set; }
        public string? Language { get; set; }
        public string? ScreenHeight { get; set; }
        public string? ScreenWidth { get; set; }
        public string? Timezone { get; set; }
        public string? UserAgent { get; set; }
        public string? ScreenColorDepth { get; set; }
        public string? JavascriptEnabled { get; set; }
    }

    public class CustomParameters
    {
        public string? Acs { get; set; }
        public int OrganisationId { get; set; }
        public int RppId { get; set; }
        public string? OppCardBin { get; set; }
    }

    public class Risk
    {
        public int Score { get; set; }
    }

    public class StandingInstruction
    {
        public string? Source { get; set; }
        public string? Type { get; set; }
        public string? Mode { get; set; }
        public string? InitialTransactionId { get; set; }
    }

}
