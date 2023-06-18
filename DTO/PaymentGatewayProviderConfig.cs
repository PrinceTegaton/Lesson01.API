namespace Lesson01.API.DTO
{
    public enum PaymentGatewayProvider
    {
        Paystack = 1,
        Flutterwave = 2,
        Stripe = 3,
        Paypal = 4
    }

    public class PaymentGatewayConfig
    {
        public PaymentGatewayProvider ActiveProvider { get; set; }
        public IEnumerable<PaymentGatewayProviderConfig> Providers { get; set; }
    }

    public class PaymentGatewayProviderConfig
    {
        public PaymentGatewayProvider Name { get; set; }
        public string AuthKey { get; set; }
        public string BaseUrl { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}