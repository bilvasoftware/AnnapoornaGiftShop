using GiftShop.Models;

namespace GiftShop.ViewModels
{
    public class ProductDetailsPageViewModel
    {
        public ProductDetailsViewModel ProductDetails { get; set; } = new();

        public List<Product> RelatedProducts { get; set; } = new();
    }
}