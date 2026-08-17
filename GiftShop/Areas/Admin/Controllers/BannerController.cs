using GiftShop.Areas.Admin.ViewModels;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IImageService _imageService;

        public BannerController(
            IBannerRepository bannerRepository,
            IImageService imageService)
        {
            _bannerRepository = bannerRepository;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerRepository.GetAllAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View(new BannerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? image = await _imageService.UploadImageAsync(
                model.ImageFile,
                "banners");

            Banner banner = new()
            {
                Title = model.Title,
                SubTitle = model.SubTitle,
                BannerImage = image,
                ButtonText = model.ButtonText,
                ButtonLink = model.ButtonLink,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                CreatedDate = DateTime.Now
            };

            await _bannerRepository.AddAsync(banner);

            TempData["Success"] = "Banner added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);

            if (banner == null)
                return NotFound();

            BannerViewModel model = new()
            {
                BannerId = banner.BannerId,
                Title = banner.Title,
                SubTitle = banner.SubTitle,
                ExistingImage = banner.BannerImage,
                ButtonText = banner.ButtonText,
                ButtonLink = banner.ButtonLink,
                DisplayOrder = banner.DisplayOrder,
                IsActive = banner.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BannerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var banner = await _bannerRepository.GetByIdAsync(model.BannerId);

            if (banner == null)
                return NotFound();

            if (model.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(banner.BannerImage))
                {
                    _imageService.DeleteImage("banners", banner.BannerImage);
                }

                banner.BannerImage = await _imageService.UploadImageAsync(
                    model.ImageFile,
                    "banners");
            }

            banner.Title = model.Title;
            banner.SubTitle = model.SubTitle;
            banner.ButtonText = model.ButtonText;
            banner.ButtonLink = model.ButtonLink;
            banner.DisplayOrder = model.DisplayOrder;
            banner.IsActive = model.IsActive;

            await _bannerRepository.UpdateAsync(banner);

            TempData["Success"] = "Banner updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _bannerRepository.GetByIdAsync(id);

            if (banner == null)
                return NotFound();

            // Delete image
            if (!string.IsNullOrEmpty(banner.BannerImage))
            {
                _imageService.DeleteImage("banners", banner.BannerImage);
            }

            await _bannerRepository.DeleteAsync(id);

            TempData["Success"] = "Banner deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}