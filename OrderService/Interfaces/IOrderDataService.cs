using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IOrderDataService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(long id);
        Task<Order> UpdateOrderByIdAsync(long id, Order order);
        Task DeleteOrderByIdAsync(long id);
    }
}