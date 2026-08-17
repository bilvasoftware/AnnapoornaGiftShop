using GiftShop.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactMessageController : Controller
    {
        private readonly IContactMessageRepository _repository;

        public ContactMessageController(IContactMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _repository.GetAllAsync();
            return View(messages);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            TempData["Success"] = "Message deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}