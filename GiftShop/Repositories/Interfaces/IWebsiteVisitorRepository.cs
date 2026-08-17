using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IWebsiteVisitorRepository
    {
        Task AddAsync(WebsiteVisitor visitor);

        Task<int> GetTotalVisitorsAsync();

        Task<int> GetTodayVisitorsAsync();

        Task<int> GetThisMonthVisitorsAsync();

        Task<int> GetTotalPageViewsAsync();
    }
}