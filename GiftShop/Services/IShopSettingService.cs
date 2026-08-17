using GiftShop.Models;

namespace GiftShop.Services
{
    public interface IShopSettingService
    {
        Task<ShopSetting> GetAsync();
    }
}