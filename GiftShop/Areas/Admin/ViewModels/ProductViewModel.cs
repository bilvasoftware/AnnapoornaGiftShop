using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public string? Brand { get; set; }

        [Required]
        [Range(0, 99999999)]
        public decimal Price { get; set; }

        public decimal? OfferPrice { get; set; }

        // GST percentage for this product
        [Range(0, 100)]
        public decimal GstPercentage { get; set; }

        // Shipping charge for this product
        [Range(0, 999999)]
        public decimal ShippingCharge { get; set; }

        public int Stock { get; set; }

        public string? Description { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ExistingImage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}