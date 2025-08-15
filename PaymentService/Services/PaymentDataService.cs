using PaymentService.Exceptions;
using PaymentService.Interfaces;
using PaymentService.Models;

namespace PaymentService.Services
{
    public class PaymentDataService : IPaymentDataService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentDataService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllPaymentsAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(long id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                throw new PaymentNotFoundException($"Payment not found with id: {id}");
            }
            return payment;
        }
    }
}