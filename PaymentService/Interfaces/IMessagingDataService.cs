using PaymentService.Models;

namespace PaymentService.Interfaces
{
    public interface IMessagingDataService
    {
        Task<Payment> CreatePaymentAsync(long orderId);
    }
}