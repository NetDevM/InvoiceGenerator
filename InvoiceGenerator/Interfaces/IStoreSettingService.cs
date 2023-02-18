using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface IStoreSettingService
    {
        Task<StoreSettings> GetStoreSettings();
        Task<bool> SaveStoreSettings(StoreSettings settings);
        Task<bool> UpdateStoreSettings(StoreSettings settings);
    }
}
