using OrderService.DTOs;

namespace OrderService.Interfaces
{
    public interface IKafkaPublisher
    {
        Task PublishMessageAsync(PaymentEventDTO paymentEventDTO);
    }
}