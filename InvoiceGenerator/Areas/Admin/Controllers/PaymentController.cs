using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Notification;
using InvoiceGenerator.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PaymentController : Controller
    {

        private readonly ICustomerService _customerService;
        private readonly ISalesInvoiceService _salesInvoiceService;
        private readonly IPaymentService _paymentService;



        public PaymentController(ICustomerService customerService, ISalesInvoiceService salesInvoiceService, IPaymentService paymentService)
        {
            _customerService = customerService;
            _salesInvoiceService = salesInvoiceService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// get all payments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Payments()
        {

            var allpayments = await _paymentService.Payments();

            return View(allpayments);
        }

        /// <summary>
        /// create payment get handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatePayment()
        {

            PaymentViewModel model = await PopulateDropdownForPayment();
            return View(model);
        }

        /// <summary>
        /// create payment post handler
        /// </summary>
        /// <param name="paymentvm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentViewModel paymentvm)
        {
            bool status = false;


            if (paymentvm == null)
                return RedirectToAction("Payments");

            if (paymentvm.SelectedSalesInvoiceId == 0 || paymentvm.SelectedCustomerId == 0)
                return RedirectToAction("Payments");


            Payment paymentmodel = new()
            {
                CustomerId = paymentvm.SelectedCustomerId,
                SalesInvoiceId = paymentvm.SelectedSalesInvoiceId,
                PaymentMode = paymentvm.SelectedPaymentMode.ToString(),
                PaymentDate = DateTime.Now,
                DueAmount = (paymentvm.Payment.GrandTotal - paymentvm.Payment.ReceivedAmount),
                GrandTotal = paymentvm.Payment.GrandTotal,
                ReceivedAmount = paymentvm.Payment.ReceivedAmount,
                PaymentNotes = paymentvm.Payment.PaymentNotes

            };

            status = await _paymentService.AddPayment(paymentmodel);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Payment Added successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            //default dropdown values
            PaymentViewModel model = await PopulateDropdownForPayment();
            return View(model);
        }

        /// <summary>
        /// edit payment get handler
        /// </summary>
        /// <param name="paymentid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditPayment(int paymentid)
        {
            PaymentViewModel model = await PopulateDropdownForPayment();
            var payment = await _paymentService.GetPaymentById(paymentid);
            model.Payment = payment;
            model.SelectedCustomerId = Convert.ToInt32(payment.CustomerId);
            model.SelectedPaymentMode = Convert.ToInt32(payment.PaymentMode);
            model.SelectedSalesInvoiceId = payment.SalesInvoiceId;
            return View(model);
        }

        /// <summary>
        /// edit payment post handler
        /// </summary>
        /// <param name="paymentvm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditPayment(PaymentViewModel paymentvm)
        {

            if (paymentvm == null)
                return RedirectToAction("Payments");

            if (paymentvm.SelectedSalesInvoiceId == 0 || paymentvm.SelectedCustomerId == 0)
                return RedirectToAction("Payments");


            Payment paymentmodel = new()
            {
                Id=paymentvm.Payment.Id,
                CustomerId = paymentvm.SelectedCustomerId,
                SalesInvoiceId = paymentvm.SelectedSalesInvoiceId,
                PaymentMode = paymentvm.SelectedPaymentMode.ToString(),
                PaymentDate = DateTime.Now,
                DueAmount = (paymentvm.Payment.GrandTotal - paymentvm.Payment.ReceivedAmount),
                GrandTotal = paymentvm.Payment.GrandTotal,
                ReceivedAmount = paymentvm.Payment.ReceivedAmount,
                PaymentNotes = paymentvm.Payment.PaymentNotes

            };

            bool status = await _paymentService.UpdatePayment(paymentmodel);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Payment Updated successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";


            //selecting the new values
            PaymentViewModel model = await PopulateDropdownForPayment();
            model.Payment = paymentmodel;
            model.SelectedCustomerId = paymentvm.SelectedCustomerId;
            model.SelectedPaymentMode = paymentvm.SelectedPaymentMode;
            model.SelectedSalesInvoiceId = paymentmodel.SalesInvoiceId;


            return View(model);
        }


        /// <summary>
        /// delete payment handler
        /// </summary>
        /// <param name="paymentid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeletePayment(int paymentid)
        {
            bool status = false;

            if (paymentid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid Payment!";
                return View("Payments");
            }

            status = await _paymentService.DeletePayment(paymentid);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Payment Deleted successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("Payments");
        }


        /// <summary>
        /// populate salesinvoices and customer dropdown
        /// </summary>
        /// <returns></returns>
        private async Task<PaymentViewModel> PopulateDropdownForPayment()
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
        /// called in paymentsdropdown.js
        /// used to populate salesinvoice based on the customer selected
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetSalesInvoiceByCustomersId(int customerid,int selectedinvoice)
        {
            var allsalesinvoiceforcustomerid = await _salesInvoiceService.GetSalesInvoiceByCustomerId(customerid);
            var selecttowdata = new SelectTwoData()
            {
                results = new List<Result>(),
            };

            //default option
            Result defaultoption = new()
            {
                id = 0,
                text = "select"
            };
            selecttowdata.results.Add(defaultoption);

            foreach (var item in allsalesinvoiceforcustomerid)
            {
                bool isselected = false;
                if (item.SalesInvoiceId == selectedinvoice)
                    isselected = true;


                Result res = new()
                {
                    id = item.SalesInvoiceId,
                    text = item?.InvoiceCode,
                    selected=isselected
                };

                selecttowdata.results.Add(res);
            }


            return Json(selecttowdata);
        }

        /// <summary>
        /// grand total based on invoice id
        /// </summary>
        /// <param name="salesinvoiceid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetGrandTotalBySalesInvoiceId(int salesinvoiceid)
        {
            float grandtotal = await _salesInvoiceService.GetGrandTotalBySalesInvoiceId(salesinvoiceid);

            if (grandtotal > 0)
                return Json(grandtotal);


            return Json("0");
        }
    }
}
