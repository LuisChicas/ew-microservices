using System;

namespace EasyWallet.Business.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Tag Tag { get; set; }
    }
}
