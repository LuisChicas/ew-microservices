using System.Collections.Generic;

namespace EasyWallet.Business.Tests.Data
{
    public class Entries
    {
        public static IEnumerable<object[]> SupportedEntryCases = new List<object[]>
        {
            new object[] { "Coffee $3", "Coffee", 3 },
            new object[] { "Coffee 3", "Coffee", 3 },
            new object[] { "2 latte coffees 3", "2 latte coffees", 3 },
            new object[] { "Coffee $3.49", "Coffee", 3.49 },
            new object[] { "Coffee 3.49", "Coffee", 3.49 },
            new object[] { "Coffee 3.49$", "Coffee", 3.49 },
            new object[] { "Coffee with 1 of sugar 3.49", "Coffee with 1 of sugar", 3.49 },
            new object[] { "Coffee  and  muffin  5.49", "Coffee and muffin", 5.49 },
            new object[] { "CofeeMachine5000 400", "CofeeMachine5000", 400 },
            new object[] { "Cofee Machine 5000 400", "Cofee Machine 5000", 400 },
        };

        public static IEnumerable<object[]> UnsupportedEntryAmountCases = new List<object[]>
        {
            new object[] { "Cappuccino $3.125" },
            new object[] { "Cappuccino $.50" },
            new object[] { "Latte 4.05.1" },
            new object[] { "Cappuccino Machine 1000.101" },
            new object[] { "Cappuccino Machine 1,0001,000" },
            new object[] { "Cappuccino Machine 1,01,000" },
            new object[] { "Cappuccino Machine 1,00.50" },
        };
    }
}
