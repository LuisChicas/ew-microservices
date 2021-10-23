using EasyWallet.Business.Dtos;
using EasyWallet.Business.Dtos.Reports;
using System.Threading.Tasks;

namespace EasyWallet.Business.Abstractions
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
