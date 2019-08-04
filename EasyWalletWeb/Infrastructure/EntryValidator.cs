using EasyWalletWeb.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWalletWeb.Infrastructure
{
    public class EntryValidator
    {
        public static EntryValidatorResult Validate(string entry, IStringLocalizer<WalletController> localizer)
        {
            if (string.IsNullOrEmpty(entry))
            {
                return new EntryValidatorResult
                {
                    IsValid = false,
                    ErrorMessage = localizer["ProvideKeywordAndAmount"]
                };
            }

            string[] entryParts = entry.Split(' ').Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (entryParts.Length < 2)
            {
                return new EntryValidatorResult
                {
                    IsValid = false,
                    ErrorMessage = localizer["ProvideKeywordAndAmount"]
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
                    ErrorMessage = localizer["ProvideValidAmount"]
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
