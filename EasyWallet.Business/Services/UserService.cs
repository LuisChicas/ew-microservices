using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Mapper;
using EasyWallet.Business.Models;
using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using System;
using System.Threading.Tasks;

namespace EasyWallet.Business.Services
{
    public class UserService : IUserService
    {
        private const int HashingWorkFactor = 13;

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var userData = await _unitOfWork.Users.GetByEmailAsync(email);
            var user = BusinessMapper.Mapper.Map<User>(userData);
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _unitOfWork.Users.EmailExistsAsync(email);
        }

        public bool VerifyPassword(string password, User user)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword(string.Empty, HashingWorkFactor);
        }

        public async Task<User> CreateUser(string email, string password, string name = null)
        {
            var userData = new UserData()
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, HashingWorkFactor),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(userData);

            var othersCategory = new CategoryData()
            {
                Name = "Others",
                UserId = userData.Id,
                CategoryTypeId = 1,
                CreatedAt = DateTime.UtcNow
            };

            var incomesCategory = new CategoryData()
            {
                Name = "Incomes",
                UserId = userData.Id,
                CategoryTypeId = 2,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Categories.AddAsync(othersCategory);
            await _unitOfWork.Categories.AddAsync(incomesCategory);

            var othersTag = new TagData()
            {
                Name = "Others",
                CategoryId = othersCategory.Id
            };

            var incomesTag = new TagData()
            {
                Name = "Incomes",
                CategoryId = incomesCategory.Id
            };

            await _unitOfWork.Tags.AddAsync(othersTag);
            await _unitOfWork.Tags.AddAsync(incomesTag);
            await _unitOfWork.CommitAsync();

            return BusinessMapper.Mapper.Map<User>(userData);
        }
    }
}
