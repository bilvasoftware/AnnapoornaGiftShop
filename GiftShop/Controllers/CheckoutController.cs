using GiftShop.Extensions;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using GiftShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public CheckoutController(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        // =========================================================
        // CHECKOUT PAGE
        // =========================================================

        public IActionResult Index()
        {
            var items =
                HttpContext.Session.GetObject<List<CartItem>>("Cart")
                ?? new List<CartItem>();

            if (!items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            CheckoutViewModel model = new()
            {
                Cart = new CartViewModel
                {
                    Items = items
                }
            };

            return View(model);
        }

        // =========================================================
        // PLACE RESERVATION
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(
            CheckoutViewModel model)
        {
            var cart =
                HttpContext.Session.GetObject<List<CartItem>>("Cart")
                ?? new List<CartItem>();

            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                model.Cart = new CartViewModel
                {
                    Items = cart
                };

                return View("Index", model);
            }

            // =====================================================
            // SAVE CUSTOMER
            // =====================================================

            int customerId =
                await _customerRepository.AddAsync(model.Customer);


            // =====================================================
            // CALCULATE TOTALS FROM CART
            // =====================================================

            decimal subTotal =
                cart.Sum(x => x.Total);

            decimal gst =
                cart.Sum(x => x.GSTAmount);

            decimal shipping =
                cart.Sum(x => x.ShippingAmount);

            decimal grandTotal =
                subTotal + gst + shipping;


            // =====================================================
            // GENERATE TOKEN NUMBER
            // =====================================================

            string tokenNumber =
                "APT" + DateTime.Now.ToString("yyyyMMddHHmmss");


            // =====================================================
            // CREATE ORDER
            // =====================================================

            Order order = new()
            {
                CustomerId = customerId,

                OrderNumber =
                    "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss"),

                TokenNumber = tokenNumber,

                OrderDate = DateTime.Now,

                SubTotal = subTotal,

                GST = gst,

                Shipping = shipping,

                GrandTotal = grandTotal,

                OrderStatus = "Reserved",

                PaymentMethod = "Pay at Shop",

                PaymentStatus = "Pending"
            };


            // =====================================================
            // CREATE ORDER ITEMS
            // =====================================================

            List<OrderItem> orderItems =
                cart.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,

                    Price = x.Price,

                    Quantity = x.Quantity,

                    Total = x.Total

                }).ToList();


            // =====================================================
            // SAVE ORDER
            // =====================================================

            await _orderRepository.SaveOrderAsync(
                order,
                orderItems);


            // =====================================================
            // CLEAR CART
            // =====================================================

            HttpContext.Session.Remove("Cart");


            // =====================================================
            // SHOW SUCCESS PAGE
            // =====================================================

            return RedirectToAction(
                nameof(Success),
                new { id = order.OrderId });
        }

        // =========================================================
        // SUCCESS
        // =========================================================

        public async Task<IActionResult> Success(int id)
        {
            var order =
                await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}