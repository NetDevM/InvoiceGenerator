using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data.Repository
{
    public class ProductRepository : IProductService
    {
        /// <summary>
        /// Dbcontext for db operations
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add Products
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// return all the products
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> Products()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Delete product for removing product implemented from Iproductservice
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProduct(int productid)
        {
            //get the product if exist
            var foundproduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.Id == productid);

            if (foundproduct is not null)
            {
                _context.Remove(foundproduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Updating product for updating product implemented from Iproductservice
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateProduct(Product product)
        {
            //get the product if exist
            var foundproduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(c => c.Id == product.Id);

            if (foundproduct is not null)
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// total products
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTotalProductsCount()
        {
            return await _context.Products.CountAsync();
        }
    }
}
