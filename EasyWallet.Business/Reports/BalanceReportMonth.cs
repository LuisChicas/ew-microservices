using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWallet.Business.Reports
{
    public struct BalanceReportMonth
    {
        public DateTime Date { get; set; }
        public decimal Balance { get; set; }

        public BalanceReportMonth(DateTime date, decimal balance)
        {
            Date = date;
            Balance = balance;
        }
    }
}
