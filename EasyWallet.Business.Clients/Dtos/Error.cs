using System.Collections;
using System.Collections.Generic;

namespace EasyWallet.Business.Clients.Dtos
{
    public class Error
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public IDictionary Data { get; private set; }

        public Error()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
