using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesInvoicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
