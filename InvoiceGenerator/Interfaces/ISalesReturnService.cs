using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ISalesReturnService
    {

        Task<SalesReturn> GeSalesReturnById(int salesreturnid);
        Task<bool> AddSalesReturn(SalesReturn salesreturn);
        Task<bool> UpdateSalesReturn(SalesReturn salesReturn);
        Task<bool> DeleteSalesReturn(int salesreturnid);
        Task<List<SalesReturn>> SalesReturns();
    }
}
