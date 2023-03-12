namespace InvoiceGenerator.Models
{
    public class InvoiceViewModel
    {
        public SalesInvoice? SalesInvoice { get; set; }

        public StoreSettings? StoreSettings { get; set; }

        public Customer? Customer { get; set; }
    }
}
