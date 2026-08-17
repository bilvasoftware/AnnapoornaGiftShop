using GiftShop.Data;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;

        public BannerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Banner>> GetAllAsync()
        {
            return await _context.Banners
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            return await _context.Banners
                .FirstOrDefaultAsync(x => x.BannerId == id);
        }

        public async Task AddAsync(Banner banner)
        {
            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Banner banner)
        {
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var banner = await GetByIdAsync(id);

            if (banner == null)
                return;

            _context.Banners.Remove(banner);

            await _context.SaveChangesAsync();
        }
    }
}