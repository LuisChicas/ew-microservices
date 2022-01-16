using EasyWallet.Business.Clients.Abstractions;
using EasyWallet.Business.Clients.Dtos;
using EasyWallet.Business.Exceptions;
using EasyWallet.Business.Services;
using EasyWallet.Business.Tests.Data;
using EasyWallet.Data.Abstractions;
using EasyWallet.Data.Entities;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyWallet.Business.Tests.Services
{
    public class EntryServiceTests
    {
        [Theory]
        [MemberData(nameof(Entries.SupportedEntryCases), MemberType = typeof(Entries))]
        public async Task AddEntry_ValidEntry_ParsesRightTagAndAmount(string entryText, string expectedTagName, decimal expectedAmount)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            var date = new DateTime();
            int userId = 1;

            await entryService.AddEntry(entryText, date, userId);

            mockUnitOfWork.Verify(x => x.Entries.AddAsync(It.Is<EntryData>(e => e.Amount == expectedAmount)));
            mockUnitOfWork.Verify(x => x.Tags.AddAsync(It.Is<TagData>(t => t.Name == expectedTagName)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddEntry_EntryNullOrEmpty_ThrowsInvalidEntryException(string entryText)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            var date = new DateTime();
            int userId = 1;

            await Assert.ThrowsAsync<InvalidEntryException>(() => entryService.AddEntry(entryText, date, userId));
        }

        [Theory]
        [InlineData("Cappuccino$3")]
        [InlineData("Latte4")]
        public async Task AddEntry_EntryWithNoSeparatedKeywordAndAmount_ThrowsInvalidEntryException(string entryText)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            var date = new DateTime();
            int userId = 1;

            await Assert.ThrowsAsync<InvalidEntryException>(() => entryService.AddEntry(entryText, date, userId));
        }

        [Fact]
        public async Task AddEntry_InvalidAmountOfMoney_ThrowsInvalidEntryAmountException()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            var date = new DateTime();
            int userId = 1;
            string entryText = "Latte 4.05.1";

            await Assert.ThrowsAsync<InvalidEntryAmountException>(() => entryService.AddEntry(entryText, date, userId));
        }

        [Fact]
        public async Task AddEntry_ValidEntry_SetsEntryDate()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            string entryText = "Gas $10";
            int userId = 1;

            var date = new DateTime(2020, 8, 11);

            await entryService.AddEntry(entryText, date, userId);

            mockUnitOfWork.Verify(x => x.Entries.AddAsync(It.Is<EntryData>(e =>
                e.Date.Year == date.Year &&
                e.Date.Month == date.Month &&
                e.Date.Day == date.Day
            )));
        }

        [Fact]
        public async Task AddEntry_EntryTextUsingSavedTag_UsesTag()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            string entryText = "Incomes $10";
            var date = new DateTime();
            int userId = Categories.DefaultIncomeCategory.UserId;

            await entryService.AddEntry(entryText, date, userId);

            mockUnitOfWork.Verify(x => x.Entries.AddAsync(It.Is<EntryData>(e =>
                e.TagId == Categories.DefaultIncomeCategory.Tags.First().Id
            )));
        }

        [Fact]
        public async Task AddEntry_EntryTextUsingNonSavedTag_CreatesNewTagWithEntryText()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var stubEntriesClient = new Mock<IEntriesClient>();
            EntryService entryService = CreateEntryService(mockUnitOfWork, stubEntriesClient);
            string entryText = "Pizza $10";
            var date = new DateTime();
            int userId = 1;

            await entryService.AddEntry(entryText, date, userId);

            mockUnitOfWork.Verify(x => x.Tags.AddAsync(It.Is<TagData>(t =>
                t.Name == "Pizza" &&
                t.CategoryId == Categories.DefaultOthersCategory.Id &&
                t.IsAutoGenerated
            )));
        }

        private EntryService CreateEntryService(Mock<IUnitOfWork> fakeUnitOfWork, Mock<IEntriesClient> fakeEntriesClient)
        {
            fakeUnitOfWork
                .Setup(x => x.Categories.GetActiveCategoriesWithTagsByUser(It.IsAny<int>()).Result)
                .Returns(Categories.DefaultCategories);

            fakeUnitOfWork.Setup(x => x.Tags.AddAsync(It.IsAny<TagData>()));

            fakeUnitOfWork.Setup(x => x.Entries.AddAsync(It.IsAny<EntryData>()));

            fakeEntriesClient.Setup(x => x.CreateEntry(It.IsAny<CreateEntryRequest>()));

            return new EntryService(fakeUnitOfWork.Object, fakeEntriesClient.Object);
        }
    }
}
