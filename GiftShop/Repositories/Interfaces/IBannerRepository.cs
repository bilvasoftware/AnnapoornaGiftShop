using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IBannerRepository
    {
        Task<List<Banner>> GetAllAsync();

        Task<Banner?> GetByIdAsync(int id);

        Task AddAsync(Banner banner);

        Task UpdateAsync(Banner banner);

        Task DeleteAsync(int id);
    }
}