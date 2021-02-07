using System;

namespace EasyWallet.Business.Exceptions
{
    public class InvalidEntryAmountException : ArgumentException
    {
        public InvalidEntryAmountException()
        {
        }

        public InvalidEntryAmountException(string message) : base(message)
        {
        }

        public InvalidEntryAmountException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
