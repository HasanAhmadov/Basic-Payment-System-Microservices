using OrderService.DTOs;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IKafkaPublisher _kafkaPublisher;
        private readonly IMessagingDataService _messagingDataService;

        public MessagingService(IKafkaPublisher kafkaPublisher, IMessagingDataService messagingDataService)
        {
            _kafkaPublisher = kafkaPublisher;
            _messagingDataService = messagingDataService;
        }

        public async Task PublishOrderCreationMessageAsync(Order order)
        {
            var paymentEventDTO = new PaymentEventDTO(order.OrderID, order.PaymentStatus);
            await _kafkaPublisher.PublishMessageAsync(paymentEventDTO);
        }

        public async Task ListenToPaymentEventAsync(PaymentEventDTO paymentEventDTO)
        {
            await _messagingDataService.UpdateOrderStatusAsync(paymentEventDTO.OrderID, paymentEventDTO.PaymentStatus);
        }
    }
}