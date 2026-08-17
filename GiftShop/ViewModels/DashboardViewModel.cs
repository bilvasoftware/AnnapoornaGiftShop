namespace GiftShop.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCategories { get; set; }

        public int TotalProducts { get; set; }

        public int TotalBrands { get; set; }

        public int TotalBanners { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalOrders { get; set; }

        public int TotalContactMessages { get; set; }

        public decimal DailySales { get; set; }

        public int TotalVisitors { get; set; }

        public int TodayVisitors { get; set; }

        public int ThisMonthVisitors { get; set; }

        public int TotalPageViews { get; set; }
        public List<DailyVisitorViewModel> DailyVisitors { get; set; } = new();
    }
}