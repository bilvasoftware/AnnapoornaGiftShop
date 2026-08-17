using System.ComponentModel.DataAnnotations;

namespace GiftShop.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100)]
        public string BrandName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? BrandLogo { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}