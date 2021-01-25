using System.Collections.Generic;

namespace EasyWallet.Business.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryTypeId { get; set; }

        public string Name { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
