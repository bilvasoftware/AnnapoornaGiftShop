using GiftShop.Areas.Admin.ViewModels;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;
        private readonly IProductImageRepository _productImageRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IImageService imageService,
            IProductImageRepository productImageRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
            _productImageRepository = productImageRepository;
        }

        // =========================================================
        // PRODUCT LIST
        // =========================================================

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();

            return View(products);
        }

        // =========================================================
        // EDIT - GET
        // =========================================================

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await LoadCategories();

            ProductViewModel model = new()
            {
                ProductId = product.ProductId,

                ProductCode = product.ProductCode,

                ProductName = product.ProductName,

                CategoryId = product.CategoryId,

                Brand = product.Brand,

                Price = product.Price,
                OfferPrice = product.OfferPrice,
                GstPercentage = product.GstPercentage,
                ShippingCharge = product.ShippingCharge,
                Stock = product.Stock,



                Description = product.Description,

                ExistingImage = product.ProductImage,

                IsFeatured = product.IsFeatured,

                IsNewArrival = product.IsNewArrival,

                IsBestSeller = product.IsBestSeller,

                IsActive = product.IsActive
            };

            return View(model);
        }

        // =========================================================
        // GALLERY - GET
        // =========================================================

        public async Task<IActionResult> Gallery(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            ProductGalleryViewModel model = new()
            {
                ProductId = product.ProductId,

                ProductName = product.ProductName,

                Gallery = await _productImageRepository
                    .GetByProductIdAsync(product.ProductId)
            };

            return View(model);
        }

        // =========================================================
        // GALLERY - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Gallery(ProductGalleryViewModel model)
        {
            if (model.Images != null && model.Images.Count > 0)
            {
                foreach (var file in model.Images)
                {
                    string? imageName =
                        await _imageService.UploadImageAsync(
                            file,
                            "products/gallery");

                    ProductImage image = new()
                    {
                        ProductId = model.ProductId,

                        ImageName = imageName!,

                        DisplayOrder = 1,

                        IsActive = true
                    };

                    await _productImageRepository.AddAsync(image);
                }
            }

            return RedirectToAction(
                nameof(Gallery),
                new { id = model.ProductId });
        }

        // =========================================================
        // DELETE GALLERY IMAGE
        // =========================================================

        public async Task<IActionResult> DeleteGalleryImage(int id)
        {
            var image =
                await _productImageRepository.GetByIdAsync(id);

            if (image == null)
                return NotFound();

            _imageService.DeleteImage(
                "products/gallery",
                image.ImageName);

            int productId = image.ProductId;

            await _productImageRepository.DeleteAsync(image);

            TempData["Success"] =
                "Gallery image deleted successfully.";

            return RedirectToAction(
                nameof(Gallery),
                new { id = productId });
        }

        // =========================================================
        // DELETE PRODUCT
        // =========================================================

        public async Task<IActionResult> Delete(int id)
        {
            var product =
                await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                _imageService.DeleteImage(
                    "products",
                    product.ProductImage);
            }

            await _productRepository.DeleteAsync(id);

            TempData["Success"] =
                "Product deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // EDIT - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();

                return View(model);
            }

            var product =
                await _productRepository.GetByIdAsync(
                    model.ProductId);

            if (product == null)
                return NotFound();

            // -----------------------------------------------------
            // Upload new image
            // -----------------------------------------------------

            if (model.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(
                    product.ProductImage))
                {
                    _imageService.DeleteImage(
                        "products",
                        product.ProductImage);
                }

                product.ProductImage =
                    await _imageService.UploadImageAsync(
                        model.ImageFile,
                        "products");
            }

            // -----------------------------------------------------
            // Update product
            // -----------------------------------------------------

            product.ProductName =
                model.ProductName;

            product.ProductCode =
                model.ProductCode;

            product.CategoryId =
                model.CategoryId;

            product.Brand =
                model.Brand;


            product.Price = model.Price;
            product.OfferPrice = model.OfferPrice;

            // GST and Shipping
            product.GstPercentage = model.GstPercentage;
            product.ShippingCharge = model.ShippingCharge;

            product.Stock = model.Stock;
           
           

            product.Description =
                model.Description;

            product.IsFeatured =
                model.IsFeatured;

            product.IsNewArrival =
                model.IsNewArrival;

            product.IsBestSeller =
                model.IsBestSeller;

            product.IsActive =
                model.IsActive;

            await _productRepository.UpdateAsync(product);

            TempData["Success"] =
                "Product updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // CREATE - GET
        // =========================================================

        public async Task<IActionResult> Create()
        {
            await LoadCategories();

            ProductViewModel model = new()
            {
                ProductCode =
                    await GenerateProductCode(),

                // Default GST
                GstPercentage = 0,

                // Default Shipping
                ShippingCharge = 0
            };

            return View(model);
        }

        // =========================================================
        // CREATE - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();

                return View(model);
            }

            // -----------------------------------------------------
            // Upload product image
            // -----------------------------------------------------

            string? imageName =
                await _imageService.UploadImageAsync(
                    model.ImageFile,
                    "products");

            // -----------------------------------------------------
            // Create product
            // -----------------------------------------------------

            Product product = new()
            {
                ProductCode =
                    model.ProductCode,

                ProductName =
                    model.ProductName,

                CategoryId =
                    model.CategoryId,

                Brand =
                    model.Brand,

                Price =
                    model.Price,

                OfferPrice =
                    model.OfferPrice,

                // -------------------------------------------------
                // GST for this particular gift
                // -------------------------------------------------

                GstPercentage =
                    model.GstPercentage,

                // -------------------------------------------------
                // Shipping for this particular gift
                // -------------------------------------------------

                ShippingCharge =
                    model.ShippingCharge,

                Stock =
                    model.Stock,

                Description =
                    model.Description,

                ProductImage =
                    imageName,

                IsFeatured =
                    model.IsFeatured,

                IsNewArrival =
                    model.IsNewArrival,

                IsBestSeller =
                    model.IsBestSeller,

                IsActive =
                    model.IsActive,

                CreatedDate =
                    DateTime.Now
            };

            await _productRepository.AddAsync(product);

            TempData["Success"] =
                "Product added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // LOAD CATEGORIES
        // =========================================================

        private async Task LoadCategories()
        {
            var categories =
                await _categoryRepository.GetAllAsync();

            ViewBag.Categories =
                new SelectList(
                    categories,
                    "CategoryId",
                    "CategoryName");
        }

        // =========================================================
        // GENERATE PRODUCT CODE
        // =========================================================

        private async Task<string> GenerateProductCode()
        {
            var products =
                await _productRepository.GetAllAsync();

            int nextNumber =
                products.Count + 1;

            return $"GFT{nextNumber:00000}";
        }
    }
}