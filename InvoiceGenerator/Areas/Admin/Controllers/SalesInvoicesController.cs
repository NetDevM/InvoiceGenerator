using InvoiceGenerator.Helper;
using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SalesInvoicesController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ISalesInvoiceService _salesinvoiceservice;
        private readonly IStoreSettingService _storesettingservice;
        public SalesInvoicesController(IProductService productService, ICustomerService customerService, ISalesInvoiceService salesinvoiceservice, IStoreSettingService storesettingservice)
        {
            _productService = productService;
            _customerService = customerService;
            _salesinvoiceservice = salesinvoiceservice;
            _storesettingservice = storesettingservice;
        }

        /// <summary>
        /// screen to display the list of sales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AllSales()
        {

            var allsales = await _salesinvoiceservice.SalesInvoices();
            return View(allsales);
        }


        /// <summary>
        /// add sales screen
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Sales()
        {
            SalesInvoice invoice = await PopulateDropdownForSalesInvoice();

            return View(invoice);
        }

        /// <summary>
        /// add sales from ajax
        /// </summary>
        /// <param name="salesorder"></param>
        /// <returns></returns>
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
                InvoicedOn = DateTime.Now,
                Notes = salesorder.Orders.Notes
            };

            //get the last salesitem
            var lastsalesinvoiceid=await _salesinvoiceservice.GetLastSalesInvoiceId();

            //generate invoice code
            salesorderinvoice.InvoiceCode = InvoiceCodeFormaterHelper.GetInvoiceCode(lastsalesinvoiceid); 

            bool status = await _salesinvoiceservice.AddInvoice(salesorderinvoice);

            if (status)
                return Json("Sales Added successfully!");
            else
                return Json("Error Occured Please try again!");

        }



        /// <summary>
        /// edit sales screen
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditSalesInvoice(int salesinvoiceid)
        {
            //populate the defaults
            SalesInvoice invoice = await PopulateDropdownForSalesInvoice();
            ViewBag.SalesInvoiceId = salesinvoiceid;
            return View(invoice);

        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<JsonResult> UpdateSalesInvoice([FromBody] SalesOrder salesorder)
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
                SalesInvoiceId = salesorder.Orders.SalesInvoiceId,
                InvoiceCode = salesorder.Orders.InvoiceCode,
                Notes = salesorder.Orders.Notes

            };


            bool status = await _salesinvoiceservice.UpdateInvoice(salesorderinvoice);

            if (status)
                return Json("Sales Updated successfully!");
            else
                return Json("Error Occured Please try again!");

        }

        /// <summary>
        /// Delete salesinvoice along with lineitems
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteSalesInvoice(int salesinvoiceid)
        {
            bool status = false;

            if (salesinvoiceid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid SalesInvoiceId!";
                return View("AllSales");
            }

            status = await _salesinvoiceservice.DeleteInvoice(salesinvoiceid);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Invoice Deleted successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("AllSales");
        }


        /// <summary>
        /// for invoice generation and print
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GenerateInvoice(int salesinvoiceid)
        {
            if (salesinvoiceid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid SalesInvoiceId!";
                return View("AllSales");
            }

            InvoiceViewModel invoicevm = new();

            invoicevm.SalesInvoice = await _salesinvoiceservice.GetSalesInvoiceById(salesinvoiceid);
            invoicevm.StoreSettings = await _storesettingservice.GetStoreSettings();
            invoicevm.Customer = await _customerService.GetCustomerById(invoicevm.SalesInvoice.CustomerId);



            return View(invoicevm);
        }




        #region Ajax Enabled Helper Methods


        /// <summary>
        /// to get the sales invoice along with lineitems for edit screen
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetSalesInvoiceWithlineItemsBySalesInvoiceId(int salesinvoiceid)
        {
            if (salesinvoiceid < 0)
                return JsonConvert.SerializeObject("Invalid SalesInvoice Id");

            var salesorder = await _salesinvoiceservice.GetSalesInvoiceById(salesinvoiceid);
            return JsonConvert.SerializeObject(salesorder);
        }




        /// <summary>
        /// called in salescart.js
        /// used in product dropdown to fetch product by id in json format ajax
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetProductByid(int productid)
        {
            var products = await _productService.GetProductById(productid);
            return Json(products);
        }



        #endregion


        #region Helper
        private async Task<SalesInvoice> PopulateDropdownForSalesInvoice()
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

            return invoice;
        }

        #endregion
    }


}
