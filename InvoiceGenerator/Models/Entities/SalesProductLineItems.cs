using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models.Entities
{
    public class SalesProductLineItems
    {
        [Key]
        public int LineItemId { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public int UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        public int SalesInvoiceId { get; set; }

        public SalesInvoice? SalesInvoice { get; set; }
    }
}
