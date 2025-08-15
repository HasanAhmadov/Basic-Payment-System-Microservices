using PaymentService.Models;

namespace PaymentService.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(long id);
        Task<Payment> CreatePaymentAsync(Payment payment);
    }
}