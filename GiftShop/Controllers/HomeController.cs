using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IShopSettingRepository _shopSettingRepository;

        public HomeController(
            IBannerRepository bannerRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IBrandRepository brandRepository,
            IProductImageRepository productImageRepository,
            IContactMessageRepository contactMessageRepository,
            IDashboardRepository dashboardRepository,
            IShopSettingRepository shopSettingRepository)
        {
            _bannerRepository = bannerRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _productImageRepository = productImageRepository;
            _contactMessageRepository = contactMessageRepository;
            _dashboardRepository = dashboardRepository;
            _shopSettingRepository = shopSettingRepository;
        }


        // =========================================================
        // HOME
        // =========================================================

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new()
            {
                Banners = (await _bannerRepository.GetAllAsync())
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList(),

                Categories = (await _categoryRepository.GetAllAsync())
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList(),

                Products = (await _productRepository.GetAllAsync())
                    .Where(x => x.IsActive)
                    .ToList(),

                Brands = (await _brandRepository.GetAllAsync())
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
            };

            return View(model);
        }


        // =========================================================
        // SHOP
        // =========================================================

        public async Task<IActionResult> Shop()
        {
            var model = new HomeViewModel
            {
                Products = await _productRepository.GetAllAsync(),
                Categories = await _categoryRepository.GetAllAsync(),
                Brands = await _brandRepository.GetAllAsync()
            };

            return View(model);
        }


        // =========================================================
        // CATEGORIES
        // =========================================================

        public async Task<IActionResult> Categories()
        {
            var model = new HomeViewModel
            {
                Categories = (await _categoryRepository.GetAllAsync())
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
            };

            return View(model);
        }


        // =========================================================
        // CATEGORY PRODUCTS
        // =========================================================

        public async Task<IActionResult> Category(int id)
        {
            var category = (await _categoryRepository.GetAllAsync())
                .FirstOrDefault(x => x.CategoryId == id);

            if (category == null)
                return NotFound();

            var products = (await _productRepository.GetAllAsync())
                .Where(x =>
                    x.CategoryId == id &&
                    x.IsActive)
                .ToList();

            ViewBag.CategoryName = category.CategoryName;

            return View(products);
        }


        // =========================================================
        // PRODUCT DETAILS
        // =========================================================

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var gallery = (await _productImageRepository
                    .GetByProductIdAsync(id))
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            var relatedProducts = (await _productRepository.GetAllAsync())
                .Where(x =>
                    x.IsActive &&
                    x.CategoryId == product.CategoryId &&
                    x.ProductId != product.ProductId)
                .Take(4)
                .ToList();

            ProductDetailsPageViewModel model = new()
            {
                ProductDetails = new ProductDetailsViewModel
                {
                    Product = product,
                    GalleryImages = gallery
                },

                RelatedProducts = relatedProducts
            };

            return View(model);
        }


        // =========================================================
        // ABOUT
        // =========================================================

        public async Task<IActionResult> About()
        {
            ViewBag.TotalProducts =
                await _dashboardRepository.GetProductCountAsync();

            ViewBag.TotalCategories =
                await _dashboardRepository.GetCategoryCountAsync();

            ViewBag.TotalCustomers =
                await _dashboardRepository.GetCustomerCountAsync();

            return View();
        }


        // =========================================================
        // CONTACT - GET
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            // Load settings saved from:
            // Admin → Settings

            var shop = await _shopSettingRepository.GetAsync();

            // If settings don't exist, use default values.
            if (shop == null)
            {
                shop = new ShopSetting
                {
                    ShopName = "Annapoorna Gift Shop",
                    Description = "Beautiful gifts for every occasion.",
                    Phone = "7200088400",
                    WhatsAppNumber = "7200088400",
                    Address = "Salem, Tamil Nadu"
                };
            }

            // Send ShopSetting to Contact.cshtml
            ViewBag.Shop = shop;

            return View(new ContactMessage());
        }


        // =========================================================
        // CONTACT - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                // IMPORTANT:
                // If validation fails, we must load ShopSetting again.
                // Otherwise the Contact page will lose the settings.

                var shop = await _shopSettingRepository.GetAsync();

                if (shop == null)
                {
                    shop = new ShopSetting
                    {
                        ShopName = "Annapoorna Gift Shop",
                        Description = "Beautiful gifts for every occasion.",
                        Phone = "7200088400",
                        WhatsAppNumber = "7200088400",
                        Address = "Salem, Tamil Nadu"
                    };
                }

                ViewBag.Shop = shop;

                return View(model);
            }


            // Save customer's message to ContactMessage table.

            await _contactMessageRepository.AddAsync(model);

            TempData["Success"] =
                "Thank you! Your message has been sent successfully.";

            return RedirectToAction(nameof(Contact));
        }
    }
}