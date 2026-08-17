using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(30)]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Brand { get; set; }

        public string? Description { get; set; }

        // Product Price
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Offer Price
        [Column(TypeName = "decimal(10,2)")]
        public decimal? OfferPrice { get; set; }

        // GST percentage for this product
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal GstPercentage { get; set; }

        // Shipping charge for this product
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999)]
        public decimal ShippingCharge { get; set; }

        public int Stock { get; set; }

        [StringLength(255)]
        public string? ProductImage { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsBestSeller { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<ProductImage>? ProductImages { get; set; }
    }
}