using System;

namespace EasyWallet.Business.Dtos
{
    public class CreateEntryRequest
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int KeywordId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
