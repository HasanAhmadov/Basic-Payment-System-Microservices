using PaymentService.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PaymentID { get; set; }

        [Required]
        public long OrderID { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }
    }
}