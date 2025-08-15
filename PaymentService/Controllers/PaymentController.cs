using Microsoft.AspNetCore.Mvc;
using PaymentService.Exceptions;
using PaymentService.Interfaces;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentDataService _paymentDataService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentDataService paymentDataService, ILogger<PaymentController> logger)
        {
            _paymentDataService = paymentDataService;
            _logger = logger;
        }

        [HttpGet("GetAllPayments")]
        public async Task<ActionResult<List<Payment>>> GetAllPayments()
        {
            try
            {
                var payments = await _paymentDataService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payments");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPaymentById/{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(long id)
        {
            try
            {
                var payment = await _paymentDataService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (PaymentNotFoundException ex)
            {
                _logger.LogWarning(ex, "Payment not found with id: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment with id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}