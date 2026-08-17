using GiftShop.Models;
using GiftShop.Repositories.Interfaces;

namespace GiftShop.Services
{
    public class ShopSettingService : IShopSettingService
    {
        private readonly IShopSettingRepository _repository;

        public ShopSettingService(
            IShopSettingRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShopSetting> GetAsync()
        {
            var setting = await _repository.GetAsync();

            return setting ?? new ShopSetting
            {
                ShopName = "Annapoorna Gift Shop",
                Description = "Beautiful gifts for every occasion.",
                Phone = "7200088400",
                WhatsAppNumber = "7200088400",
                LogoPath = "/images/logo/logo.png"
            };
        }
    }
}