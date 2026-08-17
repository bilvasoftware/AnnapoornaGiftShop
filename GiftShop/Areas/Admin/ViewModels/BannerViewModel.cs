using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Areas.Admin.ViewModels
{
    public class BannerViewModel
    {
        public int BannerId { get; set; }

        [Required(ErrorMessage = "Banner title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Subtitle")]
        public string? SubTitle { get; set; }

        [Display(Name = "Banner Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImage { get; set; }

        [Display(Name = "Button Text")]
        public string? ButtonText { get; set; }

        [Display(Name = "Button Link")]
        public string? ButtonLink { get; set; }

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}