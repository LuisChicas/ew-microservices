using EasyWallet.Business.Dtos.Reports;
using System.Threading.Tasks;

namespace EasyWallet.Business.Abstractions
{
    public interface IReportService
    {
        Task<HistoryReport> GetHistoryReport(int userId);
        Task<MonthlyReport> GetMonthlyReport(int userId);
        Task<BalanceReport> GetBalanceReport(int userId);
    }
}
