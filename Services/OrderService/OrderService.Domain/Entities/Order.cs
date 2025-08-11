using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Domain.Entities

{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderID { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        public Status Status { get; set; } = Status.PENDING;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NOT_PAID;
    }
}