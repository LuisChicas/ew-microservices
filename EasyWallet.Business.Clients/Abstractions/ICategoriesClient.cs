using EasyWallet.Business.Clients.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Business.Clients.Abstractions
{
    public interface ICategoriesClient
    {
        Task<int> CreateCategory(int userId, string name, List<string> keywordsNames);
        Task<IEnumerable<CategoryDto>> GetCategoriesByUserId(int userId);
        Task<CategoryDto> GetCategoryById(int id);
    }
}
