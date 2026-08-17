using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> SaveOrderAsync(Order order, List<OrderItem> items);

        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task UpdateAsync(Order order);

        // Search orders by Token Number or Customer Mobile Number
        Task<List<Order>> SearchAsync(string search);
    }
}