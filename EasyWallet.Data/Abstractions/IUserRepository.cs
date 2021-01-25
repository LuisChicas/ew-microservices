using EasyWallet.Data.Entities;
using System.Threading.Tasks;

namespace EasyWallet.Data.Abstractions
{
    public interface IUserRepository : IRepository<UserData>
    {
        Task<UserData> GetUserByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);
    }
}
