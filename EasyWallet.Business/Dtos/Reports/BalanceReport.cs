using System.Collections.Generic;

namespace EasyWallet.Business.Dtos.Reports
{
    public class BalanceReport
    {
        public decimal CurrentBalance { get; set; }
        public List<BalanceReportMonth> Months { get; set; }

        public BalanceReport()
        {
            Months = new List<BalanceReportMonth>();
        }
    }
}
