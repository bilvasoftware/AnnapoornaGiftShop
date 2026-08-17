using GiftShop.Models;

namespace GiftShop.Repositories.Interfaces
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetByProductIdAsync(int productId);

        Task AddAsync(ProductImage image);

        Task<ProductImage?> GetByIdAsync(int id);

        Task DeleteAsync(ProductImage image);
    }
}