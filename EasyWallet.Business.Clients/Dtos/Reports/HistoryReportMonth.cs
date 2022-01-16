using System;

namespace EasyWallet.Business.Clients.Dtos.Reports
{
    public struct HistoryReportMonth
    {
        public DateTime Date { get; set; }
        public EntryDto[] Entries { get; set; }

        public HistoryReportMonth(DateTime date, EntryDto[] entries)
        {
            Date = date;
            Entries = entries;
        }
    }
}
