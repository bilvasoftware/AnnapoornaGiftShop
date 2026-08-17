using GiftShop.Data;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class ShopSettingRepository : IShopSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public ShopSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShopSetting?> GetAsync()
        {
            return await _context.ShopSettings
                .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(ShopSetting setting)
        {
            var existing = await _context.ShopSettings
                .FirstOrDefaultAsync();

            if (existing == null)
            {
                setting.UpdatedDate = DateTime.Now;

                _context.ShopSettings.Add(setting);
            }
            else
            {
                existing.ShopName = setting.ShopName;
                existing.Description = setting.Description;
                existing.Email = setting.Email;
                existing.Phone = setting.Phone;
                existing.WhatsAppNumber = setting.WhatsAppNumber;
                existing.Address = setting.Address;
                existing.City = setting.City;
                existing.Pincode = setting.Pincode;
                existing.GoogleMapsUrl = setting.GoogleMapsUrl;
                existing.WhatsAppMessage = setting.WhatsAppMessage;
                existing.LogoPath = setting.LogoPath;
                existing.UpdatedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}