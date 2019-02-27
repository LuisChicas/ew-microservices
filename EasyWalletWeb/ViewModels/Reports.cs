using EasyWalletWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.ViewModels
{
    public class ReportsHistory
    {
        public List<KeyValuePair<DateTime, List<Entry>>> Entries { get; set; }
    }

    public class ReportsMonthly
    {
        public List<MonthlyItem> Spends { get; set; }
    }

    public class MonthlyItem
    {
        public DateTime Month { get; set; }
        public List<KeyValuePair<string, decimal>> SpendsByCategory { get; set; }

        public MonthlyItem()
        {
            SpendsByCategory = new List<KeyValuePair<string, decimal>>();
        }
    }
}
