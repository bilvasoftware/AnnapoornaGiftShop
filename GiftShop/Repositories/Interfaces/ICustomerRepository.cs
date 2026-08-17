using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> AddAsync(Customer customer);
    }
}
