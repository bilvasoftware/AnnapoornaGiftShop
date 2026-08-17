using GiftShop.Models;

namespace GiftShop.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = new();

        public List<ProductImage> GalleryImages { get; set; } = new();
    }
}