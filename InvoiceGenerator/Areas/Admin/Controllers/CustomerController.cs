using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        public  IActionResult  Index()
        {
            return View();
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
