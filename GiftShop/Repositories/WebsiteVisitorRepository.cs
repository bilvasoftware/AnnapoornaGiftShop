using GiftShop.Data;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class WebsiteVisitorRepository : IWebsiteVisitorRepository
    {
        private readonly ApplicationDbContext _context;

        public WebsiteVisitorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WebsiteVisitor visitor)
        {
            _context.WebsiteVisitors.Add(visitor);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalVisitorsAsync()
        {
            return await _context.WebsiteVisitors.CountAsync();
        }

        public async Task<int> GetTodayVisitorsAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _context.WebsiteVisitors
                .CountAsync(x =>
                    x.VisitDate >= today &&
                    x.VisitDate < tomorrow);
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
                .CountAsync(x =>
                    x.VisitDate >= startOfMonth &&
                    x.VisitDate < startOfNextMonth);
        }

        public async Task<int> GetTotalPageViewsAsync()
        {
            return await _context.WebsiteVisitors.CountAsync();
        }
    }
}