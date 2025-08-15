using OrderService.Exceptions;
using OrderService.Interfaces;
using OrderService.Models.Enums;

namespace OrderService.Services
{
    public class MessagingDataService : IMessagingDataService
    {
        private readonly IOrderRepository _orderRepository;

        public MessagingDataService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task UpdateOrderStatusAsync(long orderID, PaymentStatus paymentStatus)
        {
            var order = await _orderRepository.FindByIdAsync(orderID);
            if (order == null)
            {
                throw new OrderNotFoundException("Order with such ID is not found!");
            }

            order.PaymentStatus = paymentStatus;
            order.Status = paymentStatus == PaymentStatus.PAID ? Status.CONFIRMED : Status.FAILED;
            await _orderRepository.SaveAsync(order);
        }
    }
}