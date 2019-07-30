using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Infrastructure
{
    public class EntryValidator
    {
        private const string InvalidEntryMessage = "Please provide a tag name and an amount of money.";
        private const string InvalidAmountMessage = "Please provide a valid amount of money.";

        public static EntryValidatorResult Validate(string entry)
        {
            if (string.IsNullOrEmpty(entry))
            {
                return new EntryValidatorResult
                {
                    IsValid = false,
                    ErrorMessage = InvalidEntryMessage
                };
            }

            string[] entryParts = entry.Split(' ').Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (entryParts.Length < 2)
            {
                return new EntryValidatorResult
                {
                    IsValid = false,
                    ErrorMessage = InvalidEntryMessage
                };
            }

            string amountString = entryParts[entryParts.Length - 1];
            string keyword = string.Join(' ', entryParts.Take(entryParts.Length - 1));

            if (amountString.Contains("$"))
            {
                amountString = amountString.Replace("$", string.Empty);
            }

            decimal amount;

            if (!decimal.TryParse(amountString, out amount))
            {
                return new EntryValidatorResult
                {
                    IsValid = false,
                    ErrorMessage = InvalidAmountMessage
                };
            }

            amount = Math.Abs(amount);

            return new EntryValidatorResult
            {
                IsValid = true,
                Keyword = keyword,
                Amount = amount
            };
        }
    }
}
