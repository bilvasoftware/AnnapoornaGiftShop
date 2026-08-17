using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }
}