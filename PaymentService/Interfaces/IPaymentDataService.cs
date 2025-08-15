using PaymentService.Models;

namespace PaymentService.Interfaces
{
    public interface IPaymentDataService
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(long id);
    }
}