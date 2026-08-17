using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly IShopSettingRepository _shopSettingRepository;

        public SettingsController(
            IShopSettingRepository shopSettingRepository)
        {
            _shopSettingRepository = shopSettingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var setting = await _shopSettingRepository.GetAsync();

            if (setting == null)
            {
                setting = new ShopSetting
                {
                    ShopName = "Annapoorna Gift Shop",
                    Description = "Beautiful gifts for every occasion.",
                    Phone = "7200088400",
                    WhatsAppNumber = "7200088400"
                };
            }

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ShopSetting model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _shopSettingRepository.SaveAsync(model);

            TempData["Success"] = "Shop settings saved successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}