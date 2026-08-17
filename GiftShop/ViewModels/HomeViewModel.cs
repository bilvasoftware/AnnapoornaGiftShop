using GiftShop.Models;

namespace GiftShop.ViewModels
{
    public class HomeViewModel
    {
        public List<Banner> Banners { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public List<Product> Products { get; set; } = new();

        public List<Brand> Brands { get; set; } = new();
    }
}