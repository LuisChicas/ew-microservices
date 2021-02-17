using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWallet.Data.Repositories
{
    public class EntryRepository : Repository<EntryData>, IEntryRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public EntryRepository(EasyWalletContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EntryData>> GetActiveEntriesByUser(int userId)
        {
            return await EasyWalletContext.Categories
                .Where(c => c.UserId == userId)
                .Include(c => c.Tags).SelectMany(c => c.Tags)
                .Include(t => t.Entries).SelectMany(t => t.Entries)
                .Include(e => e.Tag.Category)
                .Where(e => e.DeletedAt == null)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public Task<EntryData> GetActiveEntryById(int id)
        {
            return EasyWalletContext.Entries
                .Where(e => e.Id == id && e.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
    }
}
