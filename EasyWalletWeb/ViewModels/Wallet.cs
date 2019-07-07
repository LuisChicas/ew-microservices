using EasyWalletWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.ViewModels
{
    public class WalletEntry
    {
        public bool PreviousEntrySaved { get; set; }
        public string Entry { get; set; }
        public DateTime Date { get; set; }
    }
}
