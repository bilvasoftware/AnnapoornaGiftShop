using GiftShop.Data;
using GiftShop.Repositories.Interfaces;
using GiftShop.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCategoryCountAsync()
            => await _context.Categories.CountAsync();

        public async Task<int> GetProductCountAsync()
            => await _context.Products.CountAsync();

        public async Task<int> GetBrandCountAsync()
            => await _context.Brands.CountAsync();

        public async Task<int> GetBannerCountAsync()
            => await _context.Banners.CountAsync();

        public async Task<int> GetCustomerCountAsync()
            => await _context.Customers.CountAsync();

        public async Task<int> GetOrderCountAsync()
            => await _context.Orders.CountAsync();

        public async Task<int> GetContactMessageCountAsync()
            => await _context.ContactMessages.CountAsync();

        public async Task<decimal> GetRevenueAsync()
        {
            return await _context.Orders.SumAsync(x => (decimal?)x.GrandTotal) ?? 0;
        }

        public async Task<int> GetTotalVisitorsAsync()
        {
            return await _context.WebsiteVisitors
                .Select(x => x.VisitorKey)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetTodayVisitorsAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.WebsiteVisitors
                .Where(x =>
                    x.VisitDate >= today &&
                    x.VisitDate < tomorrow)
                .Select(x => x.VisitorKey)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetThisMonthVisitorsAsync()
        {
            var now = DateTime.Now;

            var startOfMonth = new DateTime(
                now.Year,
                now.Month,
                1);

            var startOfNextMonth = startOfMonth.AddMonths(1);

            return await _context.WebsiteVisitors
                .Where(x =>
                    x.VisitDate >= startOfMonth &&
                    x.VisitDate < startOfNextMonth)
                .Select(x => x.VisitorKey)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetTotalPageViewsAsync()
        {
            return await _context.WebsiteVisitors.CountAsync();
        }

        public async Task<List<DailyVisitorViewModel>> GetDailyVisitorsAsync()
        {
            var startDate = DateTime.Today.AddDays(-6);
            var endDate = DateTime.Today.AddDays(1);

            var data = await _context.WebsiteVisitors
                .Where(x =>
                    x.VisitDate >= startDate &&
                    x.VisitDate < endDate)
                .GroupBy(x => x.VisitDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Visitors = g
                        .Select(x => x.VisitorKey)
                        .Distinct()
                        .Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return data
                .Select(x => new DailyVisitorViewModel
                {
                    Date = x.Date.ToString("dd MMM"),
                    Visitors = x.Visitors
                })
                .ToList();
        }

        public async Task<decimal> GetDailySalesAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.Orders
                .Where(x => x.OrderDate >= today &&
                            x.OrderDate < tomorrow)
                .SumAsync(x => (decimal?)x.GrandTotal) ?? 0;
        }
    }
}