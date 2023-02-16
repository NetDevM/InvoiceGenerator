using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> AddCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        Task<bool> DeleteCustomer(Customer customer);
        Task<List<Customer>> Customers();

    }
}
