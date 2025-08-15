using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> SaveAsync(Order order)
        {
            if (order.OrderID == 0) _context.Orders.Add(order);
        
            else _context.Orders.Update(order);

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> FindAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> FindByIdAsync(long id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<bool> ExistsByIdAsync(long id)
        {
            return await _context.Orders.AnyAsync(o => o.OrderID == id);
        }

        public async Task DeleteByIdAsync(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}