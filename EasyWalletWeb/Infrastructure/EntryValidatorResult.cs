using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Infrastructure
{
    public class EntryValidatorResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public string Keyword { get; set; }
        public decimal Amount { get; set; }
    }
}
