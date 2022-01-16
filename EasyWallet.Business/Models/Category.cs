using System.Collections.Generic;

namespace EasyWallet.Business.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
