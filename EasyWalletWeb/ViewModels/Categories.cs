using EasyWallet.Business.Models;
using System.Collections.Generic;

namespace EasyWalletWeb.ViewModels
{
    public class CategoriesIndex
    {
        public IEnumerable<Category> Categories { get; set; }
    }

    public class CategoriesForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList <Tag> Tags { get; set; }
        public bool IsNew { get; set; }
    }
}
