using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        [Required]
        public int SalesInvoiceId { get; set; }

        public string? PaymentNotes { get; set; }

        public float GrandTotal { get; set; }

        [Required]
        public float? ReceivedAmount { get; set; }

        public float? DueAmount { get; set; }

        public string? PaymentMode { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
