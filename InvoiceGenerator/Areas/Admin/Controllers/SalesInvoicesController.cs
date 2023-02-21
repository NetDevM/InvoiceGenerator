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
        public SalesInvoicesController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Sales()
        {
            //populate productlist dropdown
            SalesInvoice invoice = new()
            {
                Products = new List<SelectListItem>()
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
