using Lesson01.API.DTO;
using Lesson01.API.Interfaces;

namespace Lesson01.API.Implementations
{
    public class StripePaymentAdapter : IPaymentGatewayAdapter
    {
        private readonly PaymentGatewayProviderConfig _config;

        public StripePaymentAdapter(PaymentGatewayProviderConfig config)
        {
            _config = config;

            // create httpclient
            // configure client using _config.AuthKey and _config.BaseUrl
        }

        public async Task<Result<string>> PayWithCard(CardPaymentModel model, PaymentGatewayProvider? provider = null)
        {
            if (model == null)
            {
                return Result<string>.Failure(message: "Payload is required");
            }

            // perform model validation

            // simulate api call to Stripe payment gateway
            await Task.Delay(TimeSpan.FromSeconds(3));

            return Result<string>.Success(Guid.NewGuid().ToString(), "Payment successfully completed with Stripe");
        }
    }
}