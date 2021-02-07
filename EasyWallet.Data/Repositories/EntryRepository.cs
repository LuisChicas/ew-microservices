using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;

namespace EasyWallet.Data.Repositories
{
    public class EntryRepository : Repository<EntryData>, IEntryRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public EntryRepository(EasyWalletContext context) : base(context)
        {
        }
    }
}
