using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ISalesReportService
    {
        Task<List<SalesInvoice>> GetSalesInvoiceByDateRange(DateTime fromdate, DateTime todate);
    }
}
