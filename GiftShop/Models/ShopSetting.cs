using System.ComponentModel.DataAnnotations;

namespace GiftShop.Models
{
    public class ShopSetting
    {
        [Key]
        public int ShopSettingId { get; set; }

        [Required]
        [StringLength(150)]
        public string ShopName { get; set; } = "Annapoorna Gift Shop";

        [StringLength(500)]
        public string Description { get; set; } =
            "Beautiful gifts for every occasion.";

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [Phone]
        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(30)]
        public string? WhatsAppNumber { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? Pincode { get; set; }

        [StringLength(1000)]
        public string? GoogleMapsUrl { get; set; }

        [StringLength(1000)]
        public string? WhatsAppMessage { get; set; }

        [StringLength(2000)]
        public string? GoogleMapsEmbedUrl { get; set; }

        [StringLength(500)]
        public string? LogoPath { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}