using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models.Notification;
using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var allproducts = await _productService.Products();
            return View(allproducts);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)        {
            bool status = false;

            if (ModelState.IsValid)
            {
                product.CreatedOn = DateTime.Now;
                status = await _productService.AddProduct(product);
            }
            if (status)
                TempData[MyAlerts.SUCCESS] = "Product Added successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditProduct(int productid)
        {
            var product = await _productService.GetProductById(productid);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            bool status = await _productService.UpdateProduct(product);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Product Updated successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productid)
        {
            bool status = false;

            if (productid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid Product!";
                return View("Products");
            }

            status = await _productService.DeleteProduct(productid);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Product Deleted successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("Products");
        }

    }
}
