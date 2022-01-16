using EasyWallet.Business.Clients.Dtos;
using EasyWallet.Business.Clients.Dtos.Reports;
using System.Threading.Tasks;

namespace EasyWallet.Business.Clients.Abstractions
{
    public interface IEntriesClient
    {
        Task<int> CreateEntry(CreateEntryRequest request);
        Task<bool> DeleteEntry(int entryId);
        Task<HistoryReport> GetHistoryReport(int userId);
        Task<MonthlyReport> GetMonthlyReport(int userId);
        Task<BalanceReport> GetBalanceReport(int userId, int incomeCategoryId);
    }
}
