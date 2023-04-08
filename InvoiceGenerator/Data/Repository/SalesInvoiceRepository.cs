using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Migrations;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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

        /// <summary>
        /// delete salesinvoice and lineitems
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteInvoice(int salesinvoiceid)
        {
            //get the salesinvoice if exist
            var foundsalesinvoice = await _context.SalesInvoices.AsNoTracking().FirstOrDefaultAsync(c => c.SalesInvoiceId == salesinvoiceid);

            if (foundsalesinvoice is not null)
            {
                _context.Remove(foundsalesinvoice);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get salesinvoice by id
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        public async Task<SalesInvoice> GetSalesInvoiceById(int salesinvoiceid)
        {
            return await _context.SalesInvoices
                .Include(s => s.SalesProductLineItems)
                .AsNoTracking().FirstOrDefaultAsync(c => c.SalesInvoiceId == salesinvoiceid);

        }

        /// <summary>
        /// total sales , total revenue , recent 5 sales
        /// </summary>
        /// <returns></returns>
        public async Task<(int totalsalescount, float totalrevenue, List<SalesInvoice> latestfive)> GetSalesDataForDashboard()
        {
            var salesdata = await _context.SalesInvoices.ToListAsync();
            var totalsalescount = salesdata.Count;
            var totalrevenue = salesdata.Sum(x => x.GrandTotal);
            var recentsales = salesdata.OrderByDescending(x => x.InvoicedOn).ToList();

            return (totalsalescount, totalrevenue, recentsales);
        }

        /// <summary>
        /// all sales invoices
        /// </summary>
        /// <returns></returns>
        public async Task<List<SalesInvoice>> SalesInvoices()
        {
            return await _context.SalesInvoices.ToListAsync();
        }

        /// <summary>
        /// update salesinvoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public async Task<bool> UpdateInvoice(SalesInvoice invoice)
        {
            //get the sales if exist
            var foundsalesinvoice = await _context.SalesInvoices
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.SalesInvoiceId == invoice.SalesInvoiceId);

            //existing lineitems
            var existinglineitems = _context.SalesProductLineItems
                .AsNoTracking()
                .Where(x => x.SalesInvoiceId == invoice.SalesInvoiceId);


            if (foundsalesinvoice is not null)
            {
                using var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                //remove old salesorder items
                _context.SalesProductLineItems.RemoveRange(existinglineitems);

                //for update the main parent order item

                _context.Entry(invoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                //add new sales order items
                await _context.SalesProductLineItems.AddRangeAsync(invoice.SalesProductLineItems);
                await _context.SaveChangesAsync();


                transactionscope.Complete();
                transactionscope.Dispose();


                return true;
            }

            return false;
        }


        /// <summary>
        /// Get last salesinvoice id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetLastSalesInvoiceId()
        {
            var lastitem = await _context.SalesInvoices.OrderByDescending(s => s.SalesInvoiceId).FirstOrDefaultAsync();

            if (lastitem != null)
                return lastitem.SalesInvoiceId+1;
            else
                return 0;
        }

        /// <summary>
        /// populate by customer id
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SalesInvoice>> GetSalesInvoiceByCustomerId(int customerid)
        {
            return await _context.SalesInvoices 
              .AsNoTracking().Where(c => c.CustomerId == customerid).ToListAsync();
        }

        /// <summary>
        /// get total by salesinvoiceid
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<float> GetGrandTotalBySalesInvoiceId(int salesinvoiceid)
        {
            var salesinvoice=await _context.SalesInvoices
              .AsNoTracking().FirstOrDefaultAsync(x => x.SalesInvoiceId == salesinvoiceid);

            return salesinvoice.GrandTotal;
        }


        /// <summary>
        /// mark IsSalesReturned field true if salesreturn created
        /// mark IsSalesReturned field false if salesreturn deleted
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSalesReturnedForSalesInvoiceByID(int salesinvoiceid,bool value)
        {
            //get the salesinvoice if exist
            var foundsalesinvoice = await _context.SalesInvoices.AsNoTracking().FirstOrDefaultAsync(c => c.SalesInvoiceId == salesinvoiceid);

            if (foundsalesinvoice is not null)
            {
                
                foundsalesinvoice.IsSalesReturned = value;

                _context.Entry(foundsalesinvoice).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
