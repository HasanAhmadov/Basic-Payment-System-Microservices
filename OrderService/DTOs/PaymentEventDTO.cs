using OrderService.Models.Enums;

namespace OrderService.DTOs
{
    public class PaymentEventDTO
    {
        public long OrderID { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public PaymentEventDTO() { }

        public PaymentEventDTO(long orderID, PaymentStatus paymentStatus)
        {
            OrderID = orderID;
            PaymentStatus = paymentStatus;
        }
    }
}