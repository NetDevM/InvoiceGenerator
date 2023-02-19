using InvoiceGenerator.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class SalesInvoice
    {
        [Key]
        public int SalesInvoiceId { get; set; }

        [Required]
        public string? CustomerName { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime InvoicedOn { get; set; }

        [Required]
        public string? InvoiceCode { get; set; }

        public string? Notes { get; set; }

        [Required]
        public float SubTtotal { get; set; }

        public float Shipping { get; set; }

        public float Tax { get; set; }

        [Required]
        public float GrandTotal { get; set; }

        public List<SalesProductLineItems>? SalesProductLineItems { get; set; }

    }
}
