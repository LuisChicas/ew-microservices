using System.Collections.Generic;

namespace EasyWallet.Business.Clients.Dtos.Reports
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
