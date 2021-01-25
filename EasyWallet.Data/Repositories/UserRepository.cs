using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EasyWallet.Data.Repositories
{
    public class UserRepository : Repository<UserData>, IUserRepository
    {
        private EasyWalletContext EasyWalletContext => Context as EasyWalletContext;

        public UserRepository(EasyWalletContext context) : base(context)
        {
        }

        public Task<UserData> GetUserByEmailAsync(string email) 
        {
            return EasyWalletContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return EasyWalletContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}
