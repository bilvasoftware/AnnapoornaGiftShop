using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Areas.Admin.ViewModels
{
    public class BrandViewModel
    {
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; } = string.Empty;

        [Display(Name = "Logo")]
        public IFormFile? LogoFile { get; set; }

        public string? ExistingLogo { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}