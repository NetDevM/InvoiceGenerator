using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Migrations;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// List Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            var allcustomers = await _customerService.Customers();
            return View(allcustomers);
        }


        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                customer.CreatedOn = DateTime.Now;
                status = await _customerService.AddCustomer(customer);
            }
            if (status)
                TempData[MyAlerts.SUCCESS] = "Customer Added successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditCustomer(int customerid)
        {
            var customer = await _customerService.GetCustomerById(customerid);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer customer)
        {
            bool status = await _customerService.UpdateCustomer(customer);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Customer Updated successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int customerid)
        {
            bool status = false;

            if (customerid < 0)
            {
                TempData[MyAlerts.ERROR] = "Invalid Customer!";
                return View("Customers");
            }

            status = await _customerService.DeleteCustomer(customerid);

            if (status)
                TempData[MyAlerts.SUCCESS] = "Customer Deleted successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("Customers");
        }

    }
}
