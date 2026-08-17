using GiftShop.Areas.Admin.ViewModels;
using GiftShop.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GiftShop.Services.Interfaces;
using GiftShop.Models;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;

        public CategoryController(
            ICategoryRepository categoryRepository,
            IImageService imageService)
        {
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? imageName = await _imageService.UploadImageAsync(
                model.ImageFile,
                "categories");

            Category category = new()
            {
                CategoryName = model.CategoryName,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                CategoryImage = imageName,
                CreatedDate = DateTime.Now
            };

            await _categoryRepository.AddAsync(category);

            TempData["Success"] = "Category added successfully.";

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            CategoryViewModel model = new()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                DisplayOrder = category.DisplayOrder,
                IsActive = category.IsActive,
                ExistingImage = category.CategoryImage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = await _categoryRepository.GetByIdAsync(model.CategoryId);

            if (category == null)
                return NotFound();

            // Upload new image if selected
            if (model.ImageFile != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(category.CategoryImage))
                {
                    _imageService.DeleteImage("categories", category.CategoryImage);
                }

                // Upload new image
                category.CategoryImage = await _imageService.UploadImageAsync(
                    model.ImageFile,
                    "categories");
            }

            category.CategoryName = model.CategoryName;
            category.DisplayOrder = model.DisplayOrder;
            category.IsActive = model.IsActive;

            await _categoryRepository.UpdateAsync(category);

            TempData["Success"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            if (!string.IsNullOrEmpty(category.CategoryImage))
            {
                _imageService.DeleteImage("categories", category.CategoryImage);
            }

            await _categoryRepository.DeleteAsync(id);

            TempData["Success"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
    
}