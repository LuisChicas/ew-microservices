using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryTypeId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User User { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
