using PaymentService.DTOs;

namespace PaymentService.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishPaymentEventAsync(PaymentEventDTO paymentEvent);
    }
}