using GiftShop.Data;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveOrderAsync(
            Order order,
            List<OrderItem> items)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                item.OrderId = order.OrderId;
            }

            _context.OrderItems.AddRange(items);

            await _context.SaveChangesAsync();

            return order.OrderId;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderId == id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();
        }

        // =========================================================
        // SEARCH ORDERS
        // =========================================================

        public async Task<List<Order>> SearchAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await GetAllAsync();
            }

            search = search.Trim();

            return await _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .Where(x =>
                    x.TokenNumber.Contains(search) ||
                    (x.Customer != null &&
                     x.Customer.MobileNumber.Contains(search)))
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
        }
    }
}