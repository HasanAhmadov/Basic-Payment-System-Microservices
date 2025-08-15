using PaymentService.DTOs;

namespace PaymentService.Interfaces
{
    public interface IMessagingService
    {
        Task HandlePaymentEventAsync(PaymentEventDTO paymentEventDTO);
    }
}