using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;

namespace EasyWallet.Data.Repositories
{
    public class CategoryRepository : Repository<CategoryData>, ICategoryRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public CategoryRepository(EasyWalletContext context) : base(context)
        {
        }
    }
}
