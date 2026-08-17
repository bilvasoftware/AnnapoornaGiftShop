using Microsoft.EntityFrameworkCore;
using GiftShop.Models;

namespace GiftShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Brand> Brands => Set<Brand>();

        public DbSet<Banner> Banners => Set<Banner>();

        // NEW

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();


        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        public DbSet<WebsiteVisitor> WebsiteVisitors => Set<WebsiteVisitor>();

        public DbSet<ShopSetting> ShopSettings => Set<ShopSetting>();
    }
}