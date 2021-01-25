namespace EasyWallet.Business.Models
{
    public class Tag
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public Category Category { get; set; }
    }
}
