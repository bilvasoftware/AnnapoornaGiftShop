using System.ComponentModel.DataAnnotations;

namespace GiftShop.Models
{
    public class WebsiteVisitor
    {
        [Key]
        public int VisitorId { get; set; }

        // Identifies the browser/device as a unique visitor
        [StringLength(100)]
        public string VisitorKey { get; set; } = string.Empty;

        public string? IPAddress { get; set; }

        public string? PageUrl { get; set; }

        public string? Browser { get; set; }

        public string? Device { get; set; }

        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}