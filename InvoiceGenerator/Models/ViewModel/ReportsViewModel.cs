using InvoiceGenerator.Helper;

namespace InvoiceGenerator.Models
{
    public class ReportsViewModel
    {
        public List<SalesInvoice>?  SalesInvoice { get; set; } 

        public string CurrencyFormat { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;
    }
}
