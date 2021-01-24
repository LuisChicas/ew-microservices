using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWallet.Data.Entities
{
    public class CategoryData : Entity
    {
        public int UserId { get; set; }
        public int CategoryTypeId { get; set; }

        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public UserData User { get; set; }
        public List<TagData> Tags { get; set; }
    }
}
