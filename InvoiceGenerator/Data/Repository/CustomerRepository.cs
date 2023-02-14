using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data.Repository
{
    /// <summary>
    /// Customer Repository
    /// </summary>
    public class CustomerRepository : ICustomerService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Customers for adding customers implemented from Icustomerservice
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<bool> AddCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Delete customer for removing customers implemented from Icustomerservice
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteCustomer(Customer customer)
        {
            //get the customer if exist
            var foundcustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (foundcustomer is not null)
            {
                //soft delete
                foundcustomer.IsActive = false;
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updating customer for updating customers implemented from Icustomerservice
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            //get the customer if exist
            var foundcustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (foundcustomer is not null)
            {
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
