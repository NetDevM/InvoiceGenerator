using InvoiceGenerator.Models.Entities;

namespace InvoiceGenerator.Models
{
    public class SalesOrder
    {
        public SalesInvoice? Orders { get; set; }

        public List<SalesProductLineItems>? LineItems { get; set; }
    }
}
