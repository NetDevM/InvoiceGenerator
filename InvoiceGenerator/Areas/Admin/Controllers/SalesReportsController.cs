using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
