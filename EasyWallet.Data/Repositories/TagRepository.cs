using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;

namespace EasyWallet.Data.Repositories
{
    public class TagRepository : Repository<TagData>, ITagRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public TagRepository(EasyWalletContext context) : base(context)
        {
        }
    }
}
