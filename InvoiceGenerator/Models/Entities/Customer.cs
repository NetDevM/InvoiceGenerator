using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class Customer
    {       
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Phone { get; set; }


        public string? Address { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
