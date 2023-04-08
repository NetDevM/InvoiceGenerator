using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data.Repository
{
    public class SalesReturnRepository : ISalesReturnService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public SalesReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add SalesReturn
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> AddSalesReturn(SalesReturn salesreturn)
        {
            await _context.SalesReturns.AddAsync(salesreturn);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// delete sales return
        /// </summary>
        /// <param name="salesreturnid"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSalesReturn(int salesreturnid)
        {
            //get the salesreturn if exist
            var findsalesreturn = await _context.SalesReturns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == salesreturnid);

            if (findsalesreturn is not null)
            {
                _context.Remove(findsalesreturn);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// get salesreturn by id
        /// </summary>
        /// <param name="salesreturnid"></param>
        /// <returns></returns>
        public async Task<SalesReturn> GeSalesReturnById(int salesreturnid)
        {
            return await _context.SalesReturns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == salesreturnid);
        }

        
        /// <summary>
        /// all sales returns
        /// </summary>
        /// <returns></returns>
        public async Task<List<SalesReturn>> SalesReturns()
        {
            return await _context.SalesReturns.ToListAsync();
        }

        /// <summary>
        /// update payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateSalesReturn(SalesReturn salesreturn)
        {

            //get the salesreturn if exist
            var foundsalesreturn = await _context.SalesReturns.AsNoTracking().FirstOrDefaultAsync(c => c.Id == salesreturn.Id);

            if (foundsalesreturn is not null)
            {
                _context.Entry(salesreturn).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }
    }
}
