using System;

namespace EasyWalletWeb.ViewModels
{
    public class WalletEntry
    {
        public bool PreviousEntrySaved { get; set; }
        public string Entry { get; set; }
        public DateTime Date { get; set; }
    }
}
