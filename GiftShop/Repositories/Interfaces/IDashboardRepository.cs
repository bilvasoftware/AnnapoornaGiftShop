
using GiftShop.ViewModels;

namespace GiftShop.Repositories.Interfaces
  
{
    public interface IDashboardRepository
    {
        Task<int> GetCategoryCountAsync();

        Task<int> GetProductCountAsync();

        Task<int> GetBrandCountAsync();

        Task<int> GetBannerCountAsync();

        Task<int> GetCustomerCountAsync();

        Task<int> GetOrderCountAsync();

        Task<int> GetContactMessageCountAsync();

        Task<decimal> GetRevenueAsync();

        Task<int> GetTotalVisitorsAsync();

        Task<int> GetTodayVisitorsAsync();

        Task<int> GetThisMonthVisitorsAsync();

        Task<int> GetTotalPageViewsAsync();

        Task<decimal> GetDailySalesAsync();

        Task<List<DailyVisitorViewModel>> GetDailyVisitorsAsync();
    }
}