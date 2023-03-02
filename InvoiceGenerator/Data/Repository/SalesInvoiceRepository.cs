using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using NuGet.Configuration;

namespace InvoiceGenerator.Data.Repository
{
    public class SalesInvoiceRepository : ISalesInvoiceService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        public SalesInvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// add invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddInvoice(SalesInvoice invoice)
        {
            await _context.SalesInvoices.AddAsync(invoice);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }

        public Task<bool> DeleteInvoice(string invoiceid)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetSalesInvoiceById(string invoiceid)
        {
            throw new NotImplementedException();
        }

        public Task<List<SalesInvoice>> SalesInvoices()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInvoice(SalesInvoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}
