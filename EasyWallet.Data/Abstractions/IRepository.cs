using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyWallet.Data.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<int> AddAsync(TEntity entity);
    }
}
