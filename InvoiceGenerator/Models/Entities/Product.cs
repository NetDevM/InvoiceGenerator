using Microsoft.Build.Framework;

namespace InvoiceGenerator.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? ProductCode { get; set; }

        [Required]
        public string?  Name { get; set; }

        [Required]
        public string? Price { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
