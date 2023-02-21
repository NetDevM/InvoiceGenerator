using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SalesReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
