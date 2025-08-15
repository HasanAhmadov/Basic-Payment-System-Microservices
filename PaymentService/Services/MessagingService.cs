using PaymentService.DTOs;
using PaymentService.Interfaces;

namespace PaymentService.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IMessagingDataService _messagingDataService;
        private readonly IKafkaProducer _kafkaProducer;

        public MessagingService(IMessagingDataService messagingDataService, IKafkaProducer kafkaProducer)
        {
            _messagingDataService = messagingDataService;
            _kafkaProducer = kafkaProducer;
        }

        public async Task HandlePaymentEventAsync(PaymentEventDTO paymentEventDTO)
        {
            var payment = await _messagingDataService.CreatePaymentAsync(paymentEventDTO.OrderID);
            paymentEventDTO.PaymentStatus = payment.PaymentStatus;

            await _kafkaProducer.PublishPaymentEventAsync(paymentEventDTO);
        }
    }
}