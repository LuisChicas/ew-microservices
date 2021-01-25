using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWallet.Data.Repositories
{
    public class CategoryRepository : Repository<CategoryData>, ICategoryRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public CategoryRepository(EasyWalletContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CategoryData>> GetActiveCategoriesWithTagsByUser(int userId)
        {
            return await EasyWalletContext.Categories
                .Where(c => c.UserId == userId && c.DeletedAt == null)
                .Include(c => c.Tags)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public Task<CategoryData> GetActiveCategoryWithTagsById(int id)
        {
            return EasyWalletContext.Categories
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.Tags)
                .FirstOrDefaultAsync();
        }
    }
}
