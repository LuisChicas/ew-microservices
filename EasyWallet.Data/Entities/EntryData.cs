using System;

namespace EasyWallet.Data.Entities
{
    public class EntryData : Entity
    {
        public int TagId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public TagData Tag { get; set; }
    }
}
