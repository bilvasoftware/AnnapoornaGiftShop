using GiftShop.Repositories.Interfaces;
using GiftShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GiftShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new()
            {
                TotalCategories = await _dashboardRepository.GetCategoryCountAsync(),
                TotalProducts = await _dashboardRepository.GetProductCountAsync(),
                TotalBrands = await _dashboardRepository.GetBrandCountAsync(),
                TotalBanners = await _dashboardRepository.GetBannerCountAsync(),
                TotalCustomers = await _dashboardRepository.GetCustomerCountAsync(),
                TotalOrders = await _dashboardRepository.GetOrderCountAsync(),
                TotalContactMessages = await _dashboardRepository.GetContactMessageCountAsync(),
                DailySales = await _dashboardRepository.GetDailySalesAsync(),
                TotalVisitors = await _dashboardRepository.GetTotalVisitorsAsync(),

                TodayVisitors = await _dashboardRepository.GetTodayVisitorsAsync(),

                ThisMonthVisitors = await _dashboardRepository.GetThisMonthVisitorsAsync(),

                TotalPageViews = await _dashboardRepository.GetTotalPageViewsAsync(),

                DailyVisitors =
    await _dashboardRepository.GetDailyVisitorsAsync()
            };

            return View(model);
        }
    }
}