using OrderService.Exceptions;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Services
{
    public class OrderDataService : IOrderDataService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessagingService _messagingService;

        public OrderDataService(IOrderRepository orderRepository, IMessagingService messagingService)
        {
            _orderRepository = orderRepository;
            _messagingService = messagingService;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var savedOrder = await _orderRepository.SaveAsync(order);
            await _messagingService.PublishOrderCreationMessageAsync(savedOrder);
            return savedOrder;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.FindAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(long id)
        {
            var order = await _orderRepository.FindByIdAsync(id);
            if (order == null)
            {
                throw new OrderNotFoundException("Order with such ID is not found!");
            }
            return order;
        }

        public async Task<Order> UpdateOrderByIdAsync(long id, Order order)
        {
            var existingOrder = await _orderRepository.FindByIdAsync(id);
            if (existingOrder == null)
            {
                throw new OrderNotFoundException("Order with such ID is not found!");
            }

            existingOrder.TotalAmount = order.TotalAmount;
            return await _orderRepository.SaveAsync(existingOrder);
        }

        public async Task DeleteOrderByIdAsync(long id)
        {
            if (!await _orderRepository.ExistsByIdAsync(id))
            {
                throw new OrderNotFoundException("Order with such ID is not found!");
            }
            await _orderRepository.DeleteByIdAsync(id);
        }
    }
}