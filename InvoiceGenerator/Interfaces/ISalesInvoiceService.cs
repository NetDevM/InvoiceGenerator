using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ISalesInvoiceService
    {
        Task<bool> AddInvoice(SalesInvoice invoice);
        Task<bool> UpdateInvoice(SalesInvoice invoice);
        Task<bool> DeleteInvoice(string invoiceid);
        Task<List<SalesInvoice>> SalesInvoices();
        Task<Customer> GetSalesInvoiceById(string invoiceid);

    }
}
