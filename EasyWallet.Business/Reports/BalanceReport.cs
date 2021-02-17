using EasyWallet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyWallet.Business.Reports
{
    public class BalanceReport
    {
        public decimal CurrentBalance { get; private set; }
        public List<BalanceReportMonth> Months { get; private set; }

        public BalanceReport(EntryData[] entries)
        {
            if (entries != null && entries.Length > 0)
            {                
                (CurrentBalance, Months) = GetBalances(entries);
            }
        }

        private (decimal, List<BalanceReportMonth>) GetBalances(EntryData[] entries)
        {
            var balancesByMonth = new List<BalanceReportMonth>();
            decimal balance = 0;

            var entriesByMonth = entries
                .GroupBy(e => new DateTime(e.Date.Year, e.Date.Month, 1, 0, 0, 0))
                .OrderBy(month => month.Key);

            foreach (var month in entriesByMonth)
            {
                foreach (var entry in month)
                {
                    balance += entry.Tag.Category.CategoryTypeId == 1 ? -entry.Amount : entry.Amount;
                }

                balancesByMonth.Add(new BalanceReportMonth(
                    month.Key,
                    balance
                ));
            }

            balancesByMonth = balancesByMonth.OrderByDescending(b => b.Date).ToList();
            return (balance, balancesByMonth);
        }
    }
}
