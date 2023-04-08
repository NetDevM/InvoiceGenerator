using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class SalesReturn
    {
        public int Id { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public int SalesInvoiceId { get; set; }

        public string? InvoiceCode { get; set; }

        public string? Reason { get; set; }

        public float RefundableAmount { get; set; }

        public DateTime ReturnedDate { get; set; }
    }
}
