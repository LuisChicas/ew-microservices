using EasyWallet.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Data.Abstractions
{
    public interface ITagRepository : IRepository<TagData>
    {
        Task<IEnumerable<TagData>> GetActiveTagsByUser(int userId);
    }
}
