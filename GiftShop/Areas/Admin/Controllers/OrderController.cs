using GiftShop.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // =========================================================
        // ORDERS LIST
        // =========================================================

        public async Task<IActionResult> Index(string? search)
        {
            List<GiftShop.Models.Order> orders;

            if (string.IsNullOrWhiteSpace(search))
            {
                orders = await _orderRepository.GetAllAsync();
            }
            else
            {
                orders = await _orderRepository.SearchAsync(search);
            }

            ViewBag.Search = search;

            return View(orders);
        }

        // =========================================================
        // ORDER DETAILS
        // =========================================================

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // =========================================================
        // UPDATE ORDER STATUS
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = status;

            await _orderRepository.UpdateAsync(order);

            TempData["Success"] =
                "Order status updated successfully.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }

        // =========================================================
        // UPDATE PAYMENT STATUS
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePaymentStatus(
            int id,
            string paymentStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.PaymentStatus = paymentStatus;

            await _orderRepository.UpdateAsync(order);

            TempData["Success"] =
                "Payment status updated successfully.";

            return RedirectToAction(
                nameof(Details),
                new { id });
        }
    }
}