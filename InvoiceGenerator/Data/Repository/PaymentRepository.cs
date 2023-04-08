using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data.Repository
{
    public class PaymentRepository : IPaymentService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Payment
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> AddPayment(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Delete payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeletePayment(int paymentid)
        {
            //get the payment if exist
            var findpayment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == paymentid);

            if (findpayment is not null)
            {
                _context.Remove(findpayment);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get payment by id
        /// </summary>
        /// <param name="paymentid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Payment> GetPaymentById(int paymentid)
        {
            return await _context.Payments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == paymentid);
        }

        /// <summary>
        /// get all payments
        /// </summary>
        /// <returns></returns>
        public async Task<List<Payment>> Payments()
        {
            return await _context.Payments.ToListAsync();
        }

        /// <summary>
        /// update payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdatePayment(Payment payment)
        {

            //get the payment if exist
            var foundpayment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == payment.Id);

            if (foundpayment is not null)
            {
                _context.Entry(payment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }
    }
}
