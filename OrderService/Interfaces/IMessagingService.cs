using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IMessagingService
    {
        Task PublishOrderCreationMessageAsync(Order order);
        Task ListenToPaymentEventAsync(PaymentEventDTO paymentEventDTO);
    }
}