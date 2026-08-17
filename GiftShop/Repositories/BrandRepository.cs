using GiftShop.Data;
using GiftShop.Models;
using GiftShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands
                .FirstOrDefaultAsync(x => x.BrandId == id);
        }

        public async Task AddAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await GetByIdAsync(id);

            if (brand == null)
                return;

            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();
        }
    }
}