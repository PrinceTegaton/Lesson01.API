using Lesson01.API.DTO;

namespace Lesson01.API.Interfaces
{
    public interface IPaymentGatewayAdapter
    {
        Task<Result<string>> PayWithCard(CardPaymentModel model, PaymentGatewayProvider? provider = null);
    }
}