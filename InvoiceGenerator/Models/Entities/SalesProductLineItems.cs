using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Models.Entities
{
    public class SalesProductLineItems
    {
        [Key] 
        public int LineItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required]
        public int UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        public int SalesInvoiceId { get; set; }

        [JsonIgnore]
        public SalesInvoice? SalesInvoice { get; set; }
    }
}
