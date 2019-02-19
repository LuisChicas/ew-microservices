using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.ViewModels
{
    public class AuthSignup
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }

    public class AuthLogin
    {
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
