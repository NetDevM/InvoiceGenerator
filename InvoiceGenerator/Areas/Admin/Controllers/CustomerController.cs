using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Notification;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

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

            return RedirectToAction("Index");
        }

        //TODO
        /*
         Features
        CRUD customer
        import customer from excel
        export customer to excel
         
         */



    }
}
