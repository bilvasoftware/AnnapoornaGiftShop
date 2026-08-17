using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public string? ExistingImage { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}