using System;

namespace EasyWallet.Business.Dtos
{
    public class Entry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int KeywordId { get; set; }
        public string KeywordName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
