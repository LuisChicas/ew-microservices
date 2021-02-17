using EasyWallet.Business.Models;
using System.Collections.Generic;

namespace EasyWalletWeb.ViewModels
{
    public class CategoryForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Tag> Tags { get; set; }
        public bool IsNew { get; set; }
    }
}
