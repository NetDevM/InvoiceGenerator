using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ICustomerService
    {
        Task<int> GetTotalCustomersCount();
        Task<bool> AddCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int customerid);
        Task<List<Customer>> Customers();
        Task<Customer> GetCustomerById(int id);

    }
}
