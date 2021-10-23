using System.Collections.Generic;

namespace EasyWallet.Business.Dtos.Reports
{
    public class HistoryReport
    {
        public List<HistoryReportMonth> Months { get; set; }

        public HistoryReport()
        {
            Months = new List<HistoryReportMonth>();
        }
    }
}
