using System;

namespace EasyWallet.Business.Dtos.Reports
{
    public struct MonthlyReportMonth
    {
        public DateTime Date { get; set; }
        public MonthlyReportCategory[] Categories { get; set; }

        public MonthlyReportMonth(DateTime date, MonthlyReportCategory[] categories)
        {
            Date = date;
            Categories = categories;
        }
    }
}
