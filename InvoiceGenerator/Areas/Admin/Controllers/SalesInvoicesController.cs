using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Entities;
using InvoiceGenerator.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    //[Authorize]
    [Area("Admin")]
    public class SalesInvoicesController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ISalesInvoiceService _salesinvoiceservice;
        public SalesInvoicesController(IProductService productService, ICustomerService customerService, ISalesInvoiceService salesinvoiceservice)
        {
            _productService = productService;
            _customerService = customerService;
            _salesinvoiceservice = salesinvoiceservice;
        }

        [HttpGet]
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


        [HttpPost]
        [Consumes("application/json")]
        public async Task<JsonResult> AddSales([FromBody] SalesOrder salesorder)
        {

            //map
            SalesInvoice salesorderinvoice = new()
            {
                GrandTotal = salesorder.Orders.GrandTotal,
                PaymentMethod = salesorder.Orders.PaymentMethod,
                PaymentStatus = salesorder.Orders.PaymentStatus,
                Shipping = salesorder.Orders.Shipping,
                CustomerId = salesorder.Orders.CustomerId,
                Tax = salesorder.Orders.Tax,
                DiscountPercentage = salesorder.Orders.DiscountPercentage,
                SalesProductLineItems = salesorder.LineItems,
                InvoiceCode=Guid.NewGuid().ToString()
            };


            bool status = await _salesinvoiceservice.AddInvoice(salesorderinvoice);

            if (status)
                return Json("Sales Added successfully!");
            else
                return Json("Error Occured Please try again!");

        }


        /// <summary>
        /// called in salescart.js
        /// used in product dropdown to fetch product by id in json format
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetProductByid(int productid)
        {
            var products = await _productService.GetProductById(productid);
            return Json(products);
        }
    }


}
