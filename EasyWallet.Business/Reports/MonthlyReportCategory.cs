namespace EasyWallet.Business.Reports
{
    public struct MonthlyReportCategory
    {
        public string Name { get; set; }
        public decimal Total { get; set; }

        public MonthlyReportCategory(string name, decimal total)
        {
            Name = name;
            Total = total;
        }
    }
}
