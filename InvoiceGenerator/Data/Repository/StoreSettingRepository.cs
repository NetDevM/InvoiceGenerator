using InvoiceGenerator.Interfaces;
using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data.Repository
{
    public class StoreSettingRepository : IStoreSettingService
    {
        /// <summary>
        /// db context
        /// </summary>
        private readonly ApplicationDbContext _context;
        public StoreSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get store setting
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<StoreSettings> GetStoreSettings()
        {
            return await _context.StoreSettings.AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// save store setting
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SaveStoreSettings(StoreSettings settings)
        {
            await _context.StoreSettings.AddAsync(settings);
            int entries = await _context.SaveChangesAsync();

            if (entries > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// update store setting
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateStoreSettings(StoreSettings settings)
        {
            //get the product if exist
            var foundsettings = await _context.StoreSettings.AsNoTracking().FirstOrDefaultAsync(c => c.Id == settings.Id);

            if (foundsettings is not null)
            {
                //check if file is uploaded if not use old image
                if (string.IsNullOrEmpty(settings.ImageUrl))
                    settings.ImageUrl = foundsettings.ImageUrl;

                _context.Entry(settings).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
