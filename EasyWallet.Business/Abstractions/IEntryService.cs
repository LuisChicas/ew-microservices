using System;
using System.Threading.Tasks;

namespace EasyWallet.Business.Abstractions
{
    public interface IEntryService
    {
        Task AddEntry(string entry, DateTime date, int userId);
        Task DeleteEntry(int id);
    }
}
