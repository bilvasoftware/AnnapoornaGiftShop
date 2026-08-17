using GiftShop.Areas.Admin.ViewModels;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IImageService _imageService;

        public BrandController(
            IBrandRepository brandRepository,
            IImageService imageService)
        {
            _brandRepository = brandRepository;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepository.GetAllAsync();
            return View(brands);
        }

        public IActionResult Create()
        {
            return View(new BrandViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? logo = await _imageService.UploadImageAsync(
                model.LogoFile,
                "brands");

            Brand brand = new()
            {
                BrandName = model.BrandName,
                BrandLogo = logo,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                CreatedDate = DateTime.Now
            };

            await _brandRepository.AddAsync(brand);

            TempData["Success"] = "Brand added successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);

            if (brand == null)
                return NotFound();

            BrandViewModel model = new()
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                ExistingLogo = brand.BrandLogo,
                DisplayOrder = brand.DisplayOrder,
                IsActive = brand.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var brand = await _brandRepository.GetByIdAsync(model.BrandId);

            if (brand == null)
                return NotFound();

            if (model.LogoFile != null)
            {
                if (!string.IsNullOrEmpty(brand.BrandLogo))
                {
                    _imageService.DeleteImage("brands", brand.BrandLogo);
                }

                brand.BrandLogo = await _imageService.UploadImageAsync(
                    model.LogoFile,
                    "brands");
            }

            brand.BrandName = model.BrandName;
            brand.DisplayOrder = model.DisplayOrder;
            brand.IsActive = model.IsActive;

            await _brandRepository.UpdateAsync(brand);

            TempData["Success"] = "Brand updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);

            if (brand == null)
                return NotFound();

            // Delete logo file
            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {
                _imageService.DeleteImage("brands", brand.BrandLogo);
            }

            await _brandRepository.DeleteAsync(id);

            TempData["Success"] = "Brand deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}