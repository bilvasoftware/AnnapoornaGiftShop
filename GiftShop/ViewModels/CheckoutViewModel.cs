using GiftShop.Models;

namespace GiftShop.ViewModels
{
    public class CheckoutViewModel
    {
        public Customer Customer { get; set; } = new();

        public CartViewModel Cart { get; set; } = new();
    }
}