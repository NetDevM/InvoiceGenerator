using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Models
{
    public class StoreSettings
    {
        public int Id { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string? StoreName { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Currency { get; set; }

        [Required]
        public string? Address { get; set; }

       
        [NotMapped]
        public IFormFile? StoreImage { get; set; }

        public DateTime RegisteredOn { get; set; }


    }
}
