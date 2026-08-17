using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using GiftShop.Extensions;

namespace GiftShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>("Cart")
                   ?? new List<CartItem>();
        }

        public IActionResult Index()
        {
            List<CartItem> items = GetCart();

            CartViewModel model = new()
            {
                Items = items
            };

            return View(model);
        }

        public IActionResult Increase(int id)
        {
            var items = GetCart();

            var item = items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
            }

            HttpContext.Session.SetObject("Cart", items);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrease(int id)
        {
            var items = GetCart();

            var item = items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    items.Remove(item);
                }
            }

            HttpContext.Session.SetObject("Cart", items);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var items = GetCart();

            var item = items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                items.Remove(item);
            }

            HttpContext.Session.SetObject("Cart", items);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Add(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Don't allow adding inactive products
            if (!product.IsActive)
            {
                return RedirectToAction("Index", "Home");
            }

            // Don't allow adding products that are out of stock
            if (product.Stock <= 0)
            {
                TempData["Error"] = "This product is currently out of stock.";
                return RedirectToAction("Details", "Home", new { id });
            }

            List<CartItem> items = GetCart();

            var existing = items.FirstOrDefault(x => x.ProductId == id);

            if (existing != null)
            {
                // Increase quantity
                existing.Quantity++;

                // Refresh GST and shipping from the product
                existing.GstPercentage = product.GstPercentage;
                existing.ShippingCharge = product.ShippingCharge;
            }
            else
            {
                items.Add(new CartItem
                {
                    ProductId = product.ProductId,

                    ProductName = product.ProductName,

                    ProductImage = product.ProductImage ?? string.Empty,

                    Price = product.OfferPrice ?? product.Price,

                    Quantity = 1,

                    // IMPORTANT:
                    // Copy the values entered by Admin
                    GstPercentage = product.GstPercentage,

                    ShippingCharge = product.ShippingCharge
                });
            }

            HttpContext.Session.SetObject("Cart", items);

            return RedirectToAction(nameof(Index));
        }
    }
}