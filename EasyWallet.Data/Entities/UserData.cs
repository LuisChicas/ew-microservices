using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWallet.Data.Entities
{
    public class UserData : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<CategoryData> Categories { get; set; }
    }
}
