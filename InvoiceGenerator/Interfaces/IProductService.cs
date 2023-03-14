using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface IProductService
    {
        Task<int> GetTotalProductsCount();
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int productid);
        Task<List<Product>> Products();
        Task<Product> GetProductById(int id);
         
    }
}
