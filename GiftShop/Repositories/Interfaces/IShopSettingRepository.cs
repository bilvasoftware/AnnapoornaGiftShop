using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IShopSettingRepository
    {
        Task<ShopSetting?> GetAsync();

        Task SaveAsync(ShopSetting setting);
    }
}