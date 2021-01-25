using EasyWallet.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Business.Abstractions
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetActiveCategoriesByUser(int userId);

        Task<Category> CreateCategory(int userId, string name, IEnumerable<Tag> tags);

        Task<Category> GetActiveCategoryById(int id);

        Task UpdateCategory(int categoryId, string name, IEnumerable<Tag> tags);

        Task DeleteCategory(int id);
    }
}
