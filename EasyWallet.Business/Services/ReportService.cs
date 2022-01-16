using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Clients.Abstractions;
using EasyWallet.Business.Clients.Dtos;
using EasyWallet.Business.Clients.Dtos.Reports;
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
        private readonly IEntriesClient _entriesClient;

        public ReportService(IUnitOfWork unitOfWork, IEntriesClient entriesClient)
        {
            _unitOfWork = unitOfWork;
            _entriesClient = entriesClient;
        }
  
        public async Task<HistoryReport> GetHistoryReport(int userId)
        {
            var report = await _entriesClient.GetHistoryReport(userId);

            if (report != null && report.Months.Any())
            {
                var categories = await _unitOfWork.Categories.GetActiveCategoriesWithTagsByUser(userId);

                report.Months.ForEach(m => FillInCategoriesAndTagsNames(m.Entries, categories));
            }

            return report ?? new HistoryReport();
        }

        public async Task<MonthlyReport> GetMonthlyReport(int userId)
        {
            var report = await _entriesClient.GetMonthlyReport(userId);

            if (report != null && report.Months.Any())
            {
                var categories = await _unitOfWork.Categories.GetActiveCategoriesWithTagsByUser(userId);

                FillInMonthlyReportCategoriesNames(report.Months, categories);
            }

            return report ?? new MonthlyReport();
        }

        public async Task<BalanceReport> GetBalanceReport(int userId)
        {
            var categories = await _unitOfWork.Categories.GetActiveCategoriesWithTagsByUser(userId);
            var incomeCategory = categories.First(c => c.CategoryTypeId == (int)Models.CategoryType.Income);
            var report = await _entriesClient.GetBalanceReport(userId, incomeCategory.Id);

            return report ?? new BalanceReport();
        }

        private void FillInCategoriesAndTagsNames(IEnumerable<EntryDto> entries, IEnumerable<CategoryData> categories)
        {
            foreach(var entry in entries)
            {
                var category = categories.First(c => c.Id == entry.CategoryId);
                var tag = category.Tags.First(t => t.Id == entry.KeywordId);
                entry.CategoryName = category.Name;
                entry.KeywordName = tag.Name;
            }
        }

        private void FillInMonthlyReportCategoriesNames(List<MonthlyReportMonth> months, IEnumerable<CategoryData> categories)
        {
            foreach (var month in months)
            {
                foreach(var category in month.Categories)
                {
                    category.Name = categories.FirstOrDefault(c => c.Id == category.CategoryId).Name;
                }
            }
        }
    }
}
