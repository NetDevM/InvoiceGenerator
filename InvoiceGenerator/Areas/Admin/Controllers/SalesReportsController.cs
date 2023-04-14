using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SalesReportsController : Controller
    {

        private readonly IStoreSettingService _storeSettingService;
        private readonly ISalesReportService _salesReportService;

        public SalesReportsController(IStoreSettingService storeSettingService,
            ISalesReportService salesReportService)
        {
            _storeSettingService = storeSettingService;
            _salesReportService = salesReportService;
        }

        /// <summary>
        /// show all reports page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var storeSettings = await _storeSettingService.GetStoreSettings();

            ReportsViewModel reportsViewModel = new ReportsViewModel()
            {
                SalesInvoice = new List<SalesInvoice>(),
                CurrencyFormat = storeSettings.Currency
            };

            return View(reportsViewModel);
        }

        /// <summary>
        /// post handler of report creation
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Reports(DateTime fromdate, DateTime todate)
        {
            var storeSettings = await _storeSettingService.GetStoreSettings();

            ReportsViewModel reportsViewModel = new ReportsViewModel()
            {
                CurrencyFormat = storeSettings.Currency
            };

            reportsViewModel.SalesInvoice = await _salesReportService.GetSalesInvoiceByDateRange(fromdate, todate);

            return View(reportsViewModel);
        }

    }
}
