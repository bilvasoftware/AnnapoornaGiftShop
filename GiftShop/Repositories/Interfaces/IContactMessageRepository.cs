using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IContactMessageRepository
    {
        Task<List<ContactMessage>> GetAllAsync();

        Task<ContactMessage?> GetByIdAsync(int id);

        Task AddAsync(ContactMessage message);

        Task DeleteAsync(int id);
    }
}