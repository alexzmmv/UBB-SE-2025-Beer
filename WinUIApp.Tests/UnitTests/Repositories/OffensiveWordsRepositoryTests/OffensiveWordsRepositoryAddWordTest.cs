using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using Moq;
using MockQueryable.Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryAddWordTest
    {
        private readonly Mock<IAppDbContext> mockDatabaseContext;
        private readonly Mock<DbSet<OffensiveWord>> mockOffensiveWordsDbSet;
        private readonly OffensiveWordsRepository offensiveWordsRepository;

        private readonly List<OffensiveWord> offensiveWordsInDatabase;

        public OffensiveWordsRepositoryAddWordTest()
        {
            this.mockDatabaseContext = new Mock<IAppDbContext>();

            this.offensiveWordsInDatabase = new List<OffensiveWord>
            {
                new OffensiveWord { Word = "existingword" }
            };

            this.mockOffensiveWordsDbSet = this.offensiveWordsInDatabase.AsQueryable().BuildMockDbSet();

            this.mockDatabaseContext
                .Setup(context => context.OffensiveWords)
                .Returns(this.mockOffensiveWordsDbSet.Object);

            this.offensiveWordsRepository = new OffensiveWordsRepository(this.mockDatabaseContext.Object);
        }

        [Fact]
        public async Task AddWord_WhenWordIsNew_AddsAndSaves()
        {
            // Arrange
            string newWord = "newword";
            OffensiveWord? addedWord = null;

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Add(It.IsAny<OffensiveWord>()))
                .Callback<OffensiveWord>(word => addedWord = word);

            this.mockDatabaseContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await this.offensiveWordsRepository.AddWord(newWord);

            // Assert
            Assert.NotNull(addedWord);
            Assert.Equal(newWord, addedWord.Word);
        }

        [Fact]
        public async Task AddWord_WhenWordAlreadyExists_DoesNotCallAddOrSave()
        {
            // Arrange
            bool addCalled = false;
            bool saveCalled = false;
            string existingWord = "existingword";

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Add(It.IsAny<OffensiveWord>()))
                .Callback(() => addCalled = true);

            this.mockDatabaseContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => saveCalled = true);

            // Act
            await this.offensiveWordsRepository.AddWord(existingWord);

            // Assert
            Assert.False(addCalled);
            Assert.False(saveCalled);
        }

        [Fact]
        public async Task AddWord_WhenWordIsNullOrWhitespace_DoesNotCallAdd()
        {
            // Arrange
            string whitespaceWord = "   ";
            bool addCalled = false;

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Add(It.IsAny<OffensiveWord>()))
                .Callback(() => addCalled = true);

            // Act
            await this.offensiveWordsRepository.AddWord(whitespaceWord);

            // Assert
            Assert.False(addCalled);
        }
    }
}