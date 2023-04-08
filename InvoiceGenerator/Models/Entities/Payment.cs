using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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
