using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Models.Enums;
using PaymentService.Interfaces;

namespace PaymentService.Services
{
    public class MessagingDataService : IMessagingDataService
    {
        private readonly PaymentDbContext _dbContext;

        public MessagingDataService(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> CreatePaymentAsync(long orderId)
        {
            // Check if payment already exists
            var existingPayment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderID == orderId);
            if (existingPayment != null) return existingPayment;

            // Create new payment
            var payment = new Payment
            {
                OrderID = orderId,
                PaymentStatus = PaymentStatus.COMPLETED // or implement logic to decide PAID/FAILED
            };

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            return payment;
        }
    }
}