using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ISalesInvoiceService
    {
        Task<(int totalsalescount,float totalrevenue,List<SalesInvoice> latestfive)> GetSalesDataForDashboard();
        Task<bool> AddInvoice(SalesInvoice invoice);
        Task<bool> UpdateInvoice(SalesInvoice invoice);
        Task<bool> DeleteInvoice(int invoiceid);
        Task<List<SalesInvoice>> SalesInvoices();
        Task<SalesInvoice> GetSalesInvoiceById(int invoiceid);
        Task<int> GetLastSalesInvoiceId();
        Task<List<SalesInvoice>> GetSalesInvoiceByCustomerId(int customerid);
        Task<float> GetGrandTotalBySalesInvoiceId(int salesinvoiceid);

    }
}
