using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ISalesInvoiceService
    {
        Task<bool> AddInvoice(SalesInvoice invoice);
        Task<bool> UpdateInvoice(SalesInvoice invoice);
        Task<bool> DeleteInvoice(int invoiceid);
        Task<List<SalesInvoice>> SalesInvoices();
        Task<SalesInvoice> GetSalesInvoiceById(int invoiceid);

    }
}
