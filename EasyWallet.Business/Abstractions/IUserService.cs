using EasyWallet.Business.Models;
using System.Threading.Tasks;

namespace EasyWallet.Business.Abstractions
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

        bool VerifyPassword(string password, User user);

        void FakeHash();

        Task<User> CreateUser(string email, string password, string name = null);
    }
}
