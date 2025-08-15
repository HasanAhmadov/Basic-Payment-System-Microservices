using OrderService.Models;

namespace OrderService.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> SaveAsync(Order order);
        Task<List<Order>> FindAllAsync();
        Task<Order?> FindByIdAsync(long id);
        Task<bool> ExistsByIdAsync(long id);
        Task DeleteByIdAsync(long id);
    }
}