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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Customer customer)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                customer.CreatedOn = DateTime.Now;
                status = await _customerService.AddCustomer(customer);
            }
            if (status)
                TempData[MyAlerts.SUCCESS] = "Customer successfully!";
            else
                TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            return RedirectToAction("Index");
        }

        //TODO
        /*
         Features
        allow to add customer types e.g walkin,existing,premium ( separate screen) crud for this ( show dropdown along with plus button
        and tell if the value not exist in the dropdown click on add to add the customer type)
        CRUD customer
        import customer from excel
        export customer to excel
         
         */


        //[HttpGet]
        //public async Task<IActionResult> CreateCustomer()
        //{

        //}


    }
}
