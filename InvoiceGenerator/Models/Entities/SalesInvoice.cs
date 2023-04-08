using InvoiceGenerator.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Models
{
    public class SalesInvoice
    {
        [Key]
        public int SalesInvoiceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime InvoicedOn { get; set; }

        [Required]
        public string? InvoiceCode { get; set; }

        public string? Notes { get; set; }

        public float DiscountPercentage { get; set; }
 

        public float Shipping { get; set; }

        public float Tax { get; set; }
         

        [Required]
        public float GrandTotal { get; set; }

        public bool IsSalesReturned { get; set; }

        public List<SalesProductLineItems>? SalesProductLineItems { get; set; }

        #region For View
        [JsonIgnore]
        [NotMapped]
        public List<SelectListItem>? Products { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<SelectListItem>? Customers { get; set; } 
        #endregion

    }
}
