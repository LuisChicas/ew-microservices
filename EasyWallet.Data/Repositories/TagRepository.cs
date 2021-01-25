using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWallet.Data.Repositories
{
    public class TagRepository : Repository<TagData>, ITagRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public TagRepository(EasyWalletContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TagData>> GetActiveTagsByUser(int userId)
        {
            return await EasyWalletContext.Categories
                .Where(c => c.UserId == userId && c.DeletedAt == null)
                .SelectMany(c => c.Tags)
                .Where(t => t.DeletedAt == null)
                .ToListAsync();
        }
    }
}
