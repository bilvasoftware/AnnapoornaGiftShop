using System.ComponentModel.DataAnnotations;

namespace GiftShop.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(250)]
        public string? SubTitle { get; set; }

        [StringLength(255)]
        public string? BannerImage { get; set; }

        [StringLength(50)]
        public string? ButtonText { get; set; }

        [StringLength(250)]
        public string? ButtonLink { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}