using EasyWallet.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Data.Abstractions
{
    public interface ICategoryRepository : IRepository<CategoryData>
    {
        Task<IEnumerable<CategoryData>> GetActiveCategoriesWithTagsByUser(int userId);

        Task<CategoryData> GetActiveCategoryWithTagsById(int id);
    }
}
