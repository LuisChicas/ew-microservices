using EasyWalletWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.ViewModels
{
    public class CategoriesIndex
    {
        public IList<Category> Categories { get; set; }
    }

    public class CategoriesForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Tag> Tags { get; set; }
        public bool IsNew { get; set; }
    }
}
