using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Category Category { get; set; }
        public List<Entry> Entries { get; set; }
    }
}
