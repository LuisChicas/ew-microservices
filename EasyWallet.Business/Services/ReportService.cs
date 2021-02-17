using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Reports;
using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWallet.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
  
        public async Task<HistoryReport> GetHistoryReport(int userId)
        {
            IEnumerable<EntryData> entries = await _unitOfWork.Entries.GetActiveEntriesByUser(userId);
            return new HistoryReport(entries);
        }

        public async Task<MonthlyReport> GetMonthlyReport(int userId)
        {
            IEnumerable<EntryData> entries = await _unitOfWork.Entries.GetActiveEntriesByUser(userId);
            return new MonthlyReport(entries);
        }

        public async Task<BalanceReport> GetBalanceReport(int userId)
        {
            IEnumerable<EntryData> entries = await _unitOfWork.Entries.GetActiveEntriesByUser(userId);
            return new BalanceReport(entries.ToArray());
        }
    }
}
