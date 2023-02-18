using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Migrations;
using InvoiceGenerator.Models;
using InvoiceGenerator.Models.Notification;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly IStoreSettingService _storeSettingService;
        private IWebHostEnvironment _webHostEnvironment;

        public SettingsController(IStoreSettingService settingService, IWebHostEnvironment env)
        {
            _storeSettingService = settingService;
            _webHostEnvironment = env;
        }

        [HttpGet]
        public async Task<IActionResult> StoreSetting()
        {
            //get settings if available
            var storesettings = await _storeSettingService.GetStoreSettings();

            if (storesettings is not null)
                return View(storesettings);
            else
                return View(new StoreSettings());
        }

        [HttpPost]
        public async Task<IActionResult> StoreSetting(StoreSettings storesettings)
        {
            string filepathtostoreindb = string.Empty;

            #region Handling Image
            if (storesettings?.StoreImage?.Length > 0)
            {
                //get path directory to save
                string path = Path.GetFullPath(Path.Combine(_webHostEnvironment.WebRootPath, "images"));

                //check if exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //create file from stream and copy
                using var fileStream = new FileStream(Path.Combine(path, storesettings.StoreImage.FileName), FileMode.Create);
                await storesettings.StoreImage.CopyToAsync(fileStream);

                //if file is posted
                filepathtostoreindb = $"/images/{storesettings?.StoreImage?.FileName}";
            }

            #endregion


            //filename 
            StoreSettings settings = new()
            {
                Id = storesettings.Id,
                Address = storesettings?.Address,
                Currency = storesettings?.Currency,
                Email = storesettings?.Email,
                Phone = storesettings?.Phone,
                StoreName = storesettings?.StoreName,
                RegisteredOn = DateTime.Now,
                ImageUrl = filepathtostoreindb
            };

            //check if exist
            if (storesettings?.Id > 0)
            {
                //update
                bool status = await _storeSettingService.UpdateStoreSettings(settings);

                if (status)
                    TempData[MyAlerts.SUCCESS] = "Settings Updated successfully!";
                else
                    TempData[MyAlerts.ERROR] = "Error Occured Please try again!";
            }
            else
            {
                //add 
                bool status = await _storeSettingService.SaveStoreSettings(settings);

                if (status)
                    TempData[MyAlerts.SUCCESS] = "Settings Saved successfully!";
                else
                    TempData[MyAlerts.ERROR] = "Error Occured Please try again!";

            }
            return View(storesettings);

        }
    }
}
