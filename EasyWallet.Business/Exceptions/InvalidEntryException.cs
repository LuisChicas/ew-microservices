using System;

namespace EasyWallet.Business.Exceptions
{
    public class InvalidEntryException : ArgumentException
    {
        public InvalidEntryException()
        {
        }

        public InvalidEntryException(string message) : base(message)
        {
        }

        public InvalidEntryException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
