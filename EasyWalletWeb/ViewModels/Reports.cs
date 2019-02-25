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
}
