using GiftShop.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Areas.Admin.ViewModels
{
    public class ProductGalleryViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Gallery Images")]
        public List<IFormFile>? Images { get; set; }

        public List<ProductImage> Gallery { get; set; } = new();
    }
}