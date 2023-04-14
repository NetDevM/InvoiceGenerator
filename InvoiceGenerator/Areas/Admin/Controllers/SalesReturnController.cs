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
    public class SalesReturnController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly ISalesInvoiceService _salesInvoiceService;
        private readonly ISalesReturnService _salesReturnService;

        public SalesReturnController(ICustomerService customerService, ISalesInvoiceService salesInvoiceService, ISalesReturnService salesReturnService)
        {
            _customerService = customerService;
            _salesInvoiceService = salesInvoiceService;
            _salesReturnService = salesReturnService;

        }


        /// <summary>
        /// all sales return list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SalesReturns()
        {
            var allsalesreturn = await _salesReturnService.SalesReturns();

            return View(allsalesreturn);


        }

        /// <summary>
        /// create new sales return
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateSalesReturn()
        {

            PaymentViewModel model = await PopulateDropdownFoSalesReturn();
            return View(model);
        }


        /// <summary>
        /// post handler of sales return creation
        /// </summary>
        /// <param name="salesreturnvm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSalesReturn(PaymentViewModel salesreturnvm)
        {
            bool status = false;


            if (salesreturnvm == null)
                return RedirectToAction("SalesReturns");

            if (salesreturnvm.SelectedSalesInvoiceId == 0 || salesreturnvm.SelectedCustomerId == 0)
                return RedirectToAction("SalesReturns");


            SalesReturn salesreturnmodel = new()
            {
                CustomerId = salesreturnvm.SelectedCustomerId,
                SalesInvoiceId = salesreturnvm.SelectedSalesInvoiceId,
                ReturnedDate = DateTime.Now,
                RefundableAmount = salesreturnvm.SalesReturn.RefundableAmount,
                Reason = salesreturnvm.SalesReturn.Reason,
                InvoiceCode = salesreturnvm.SalesReturn.InvoiceCode,

            };

            status = await _salesReturnService.AddSalesReturn(salesreturnmodel);

            if (status) 
                TempData[MyAlerts.SUCCESS] = "SalesReturn Added successfully!";
             
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            //default dropdown values
            PaymentViewModel model = await PopulateDropdownFoSalesReturn();
            return View(model);
        }


        /// <summary>
        /// get screen of edit sales return
        /// </summary>
        /// <param name="salesreturnid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditSalesReturn(int salesreturnid)
        {
            PaymentViewModel model = await PopulateDropdownFoSalesReturn();
            var salesreturn = await _salesReturnService.GeSalesReturnById(salesreturnid);
            model.SalesReturn = salesreturn;
            model.SelectedCustomerId = Convert.ToInt32(salesreturn.CustomerId);
            model.SelectedSalesInvoiceId = salesreturn.SalesInvoiceId;
            return View(model);
        }


        /// <summary>
        /// post handler of edit sales return
        /// </summary>
        /// <param name="salesreturnvm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditSalesReturn(PaymentViewModel salesreturnvm)
        {

            if (salesreturnvm == null)
                return RedirectToAction("SalesReturns");

            if (salesreturnvm.SelectedSalesInvoiceId == 0 || salesreturnvm.SelectedCustomerId == 0)
                return RedirectToAction("SalesReturns");


            SalesReturn salesreturnmodel = new()
            {
                Id = salesreturnvm.SalesReturn.Id,
                CustomerId = salesreturnvm.SelectedCustomerId,
                SalesInvoiceId = salesreturnvm.SelectedSalesInvoiceId,
                RefundableAmount = salesreturnvm.SalesReturn.RefundableAmount,
                Reason = salesreturnvm.SalesReturn.Reason,
                InvoiceCode = salesreturnvm.SalesReturn.InvoiceCode,

            };

            bool status = await _salesReturnService.UpdateSalesReturn(salesreturnmodel);

            if (status)
                TempData[MyAlerts.SUCCESS] = "SalesReturn Updated successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";


            //selecting the new values
            PaymentViewModel model = await PopulateDropdownFoSalesReturn();
            model.SalesReturn = salesreturnmodel;
            model.SelectedCustomerId = Convert.ToInt32(salesreturnmodel.CustomerId);
            model.SelectedSalesInvoiceId = salesreturnmodel.SalesInvoiceId;


            return View(model);
        }



        /// <summary>
        /// delete sales return from table
        /// </summary>
        /// <param name="salesreturnid"></param>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteSalesReturn(int salesreturnid,int salesinvoiceid)
        {
            bool status = false;

            if (salesreturnid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid SalesReturn!";
                return View("SalesReturns");
            }

            status = await _salesReturnService.DeleteSalesReturn(salesreturnid);

            if (status) 
                TempData[MyAlerts.SUCCESS] = "SalesReturn Deleted successfully!"; 
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("SalesReturns");
        }


        /// <summary>
        /// populate customer and invoice dropdown
        /// </summary>
        /// <returns></returns>
        private async Task<PaymentViewModel> PopulateDropdownFoSalesReturn()
        {
            //populate productlist dropdown
            PaymentViewModel paymentvm = new()
            {
                SalesInvoices = new List<SelectListItem>(),
                Customers = new List<SelectListItem>()
            };

            var invoices = await _salesInvoiceService.SalesInvoices();
            foreach (var invoice in invoices)
            {
                paymentvm.SalesInvoices.Add(new SelectListItem()
                {
                    Text = invoice.InvoiceCode,
                    Value = invoice.SalesInvoiceId.ToString(),
                });
            }

            var customers = await _customerService.Customers();
            foreach (var customer in customers)
            {
                paymentvm.Customers.Add(new SelectListItem()
                {
                    Text = customer.Name,
                    Value = customer.Id.ToString(),
                });
            }

            return paymentvm;
        }


        /// <summary>
        /// grand total based on invoice id
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetGrandTotalWithLineItemsBySalesInvoiceId(int salesinvoiceid)
        {
            var salesorder = await _salesInvoiceService.GetSalesInvoiceById(salesinvoiceid);
            return JsonConvert.SerializeObject(salesorder);

        }

    }
}
