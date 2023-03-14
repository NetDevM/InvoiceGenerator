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
        /// return all the customers
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> Customers()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Delete customer for removing customers implemented from Icustomerservice
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteCustomer(int customerid)
        {
            //get the customer if exist
            var foundcustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == customerid);

            if (foundcustomer is not null)
            {
                _context.Remove(foundcustomer);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// get customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// total customer count
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalCustomersCount()
        {
            return await _context.Customers.CountAsync();
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
            var foundcustomer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == customer.Id);

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
