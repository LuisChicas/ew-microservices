﻿using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Dtos;
using EasyWallet.Business.Exceptions;
using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EasyWallet.Business.Services
{
    public class EntryService : IEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntriesClient _entriesClient;

        public EntryService(IUnitOfWork unitOfWork, IEntriesClient entriesClient)
        {
            _unitOfWork = unitOfWork;
            _entriesClient = entriesClient;
        }

        public async Task AddEntry(string entryText, DateTime date, int userId)
        {
            (string keyword, decimal amount) = ParseEntry(entryText);

            var categories = await _unitOfWork.Categories.GetActiveCategoriesWithTagsByUser(userId);

            TagData tag = categories
                .SelectMany(c => c.Tags)
                .Where(t => t.Name.ToLower() == keyword.ToLower())
                .FirstOrDefault();

            if (tag == default(TagData))
            {
                CategoryData othersCategory = categories.First(c => c.Name == "Others" || c.Name == "Otros");

                tag = new TagData()
                {
                    Name = keyword,
                    CategoryId = othersCategory.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsAutoGenerated = true
                };

                await _unitOfWork.Tags.AddAsync(tag);
                await _unitOfWork.CommitAsync();
            }

            var createEntryRequest = new CreateEntryRequest
            {
                UserId = userId,
                CategoryId = tag.CategoryId,
                KeywordId = tag.Id,
                Amount = amount,
                Date = date
            };

            await _entriesClient.CreateEntry(createEntryRequest);
        }

        public async Task DeleteEntry(int id)
        {
            await _entriesClient.DeleteEntry(id);
        }

        private (string keyword, decimal amount) ParseEntry(string entry)
        {
            if (string.IsNullOrEmpty(entry))
            {
                throw new InvalidEntryException("Argument was null or empty.");
            }

            string[] entryParts = entry.Split(' ').Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (entryParts.Length < 2)
            {
                throw new InvalidEntryException("Missing keyword, amount, or a whitespace between them.");
            }

            string amountString = entryParts[entryParts.Length - 1].Replace("$", string.Empty);
            string keyword = string.Join(' ', entryParts.Take(entryParts.Length - 1));

            decimal amount;

            if (!decimal.TryParse(amountString, out amount))
            {
                throw new InvalidEntryAmountException("Amount was not a valid quantity of money.");
            }

            amount = Math.Abs(amount);

            return (keyword, amount);
        }
    }
}
