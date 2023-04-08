using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator.Models
{
    public class PaymentViewModel
    {
        public Payment? Payment { get; set; }

        public int SelectedPaymentMode { get; set; }

        public List<SelectListItem>? Customers { get; set; }
 
        public List<SelectListItem>? SalesInvoices { get; set; }

        public int SelectedCustomerId { get; set; }

        public int SelectedSalesInvoiceId { get; set; }

    }
}
