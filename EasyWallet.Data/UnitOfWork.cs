using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyWallet.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);
        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);
        public ITagRepository Tags => _tagRepository = _tagRepository ?? new TagRepository(_context);
        public IEntryRepository Entries => _entryRepository = _entryRepository ?? new EntryRepository(_context);

        private readonly EasyWalletContext _context;
        private UserRepository _userRepository;
        private CategoryRepository _categoryRepository;
        private TagRepository _tagRepository;
        private EntryRepository _entryRepository;

        public UnitOfWork(EasyWalletContext context)
        {
            _context = context;
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
