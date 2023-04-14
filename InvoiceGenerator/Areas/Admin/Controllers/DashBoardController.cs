using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class DashBoardController : Controller
    {

        private readonly ISalesInvoiceService _salesinvoiceservice;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IStoreSettingService _storeSettingService;

        public DashBoardController(ISalesInvoiceService salesInvoiceService, IProductService productService, ICustomerService customerService,IStoreSettingService storeSettingService)
        {
            _salesinvoiceservice = salesInvoiceService;
            _productService = productService;
            _customerService = customerService;
            _storeSettingService= storeSettingService;

        }

        /// <summary>
        /// get the dashboard data for the cards
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DashBoardViewModel dashboardvm = new DashBoardViewModel();


            (int totalsalescount, float totalrevenue, List<SalesInvoice> latestfive) = await _salesinvoiceservice.GetSalesDataForDashboard();
            dashboardvm.SalesInvoices = latestfive;
            dashboardvm.TotalRevenue = totalrevenue;
            dashboardvm.TotalSales = totalsalescount;

            dashboardvm.TotalCustomers = await _customerService.GetTotalCustomersCount();
            dashboardvm.TotalProducts = await _productService.GetTotalProductsCount();
            dashboardvm.StoreSettings = await _storeSettingService.GetStoreSettings();

            return View(dashboardvm);
        }
    }
}
