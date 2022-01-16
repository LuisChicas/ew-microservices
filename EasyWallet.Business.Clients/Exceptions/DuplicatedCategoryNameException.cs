using System;

namespace EasyWallet.Business.Clients.Exceptions
{
    public class DuplicatedCategoryNameException : ArgumentException
    {
        public DuplicatedCategoryNameException()
        {
        }

        public DuplicatedCategoryNameException(string message) : base(message)
        {
        }

        public DuplicatedCategoryNameException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
