using EasyWallet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyWallet.Business.Reports
{
    public class MonthlyReport
    {
        public MonthlyReportMonth[] Months { get; private set; }

        public MonthlyReport(IEnumerable<EntryData> entries)
        {
            Months = entries
                .GroupBy(e => new DateTime(e.Date.Year, e.Date.Month, 1))
                .Select(e => new MonthlyReportMonth(e.Key, GetCategoriesTotals(e.ToArray())))
                .ToArray();
        }

        private MonthlyReportCategory[] GetCategoriesTotals(IEnumerable<EntryData> entries)
        {
            return entries
                .GroupBy(e => e.Tag.Category)
                .Select(c =>
                {
                    decimal total = 0;

                    foreach (var entry in c.ToArray())
                    {
                        total += entry.Amount;
                    }

                    return new MonthlyReportCategory(c.Key.Name, total);
                }).ToArray();
        }
    }
}
