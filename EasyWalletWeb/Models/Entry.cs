using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public Tag Tag { get; set; }
    }
}
