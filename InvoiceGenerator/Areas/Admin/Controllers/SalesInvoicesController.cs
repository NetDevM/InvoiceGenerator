using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SalesInvoicesController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        public SalesInvoicesController(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Sales()
        {
            //populate productlist dropdown
            SalesInvoice invoice = new()
            {
                Products = new List<SelectListItem>(),
                Customers = new List<SelectListItem>()
            };

            var products = await _productService.Products();
            foreach (var product in products)
            {
                invoice.Products.Add(new SelectListItem()
                {
                    Text = product.Name,
                    Value = product.Id.ToString(),
                });
            }

            var customers = await _customerService.Customers();
            foreach (var customer in customers)
            {
                invoice.Customers.Add(new SelectListItem()
                {
                    Text = customer.Name,
                    Value = customer.Id.ToString(),
                });
            }

            return View(invoice);
        }


        [HttpGet]
        public async Task<JsonResult> GetProductByid(int productid)
        {
            var products = await _productService.GetProductById(productid);
            return Json(products);
        }
    }
}
