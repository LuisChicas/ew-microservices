using System.Collections.Generic;

namespace EasyWallet.Business.Clients.Dtos
{
    public class CreateCategoryRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<string> KeywordsNames { get; set; }
    }
}
