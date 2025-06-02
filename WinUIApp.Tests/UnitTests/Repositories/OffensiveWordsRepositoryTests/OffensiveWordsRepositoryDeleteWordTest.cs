using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryDeleteWordTest
    {
        private readonly Mock<IAppDbContext> mockDatabaseContext;
        private readonly Mock<DbSet<OffensiveWord>> mockOffensiveWordsDbSet;
        private readonly OffensiveWordsRepository offensiveWordsRepository;

        private readonly List<OffensiveWord> offensiveWordsInDatabase;

        public OffensiveWordsRepositoryDeleteWordTest()
        {
            this.mockDatabaseContext = new Mock<IAppDbContext>();

            this.offensiveWordsInDatabase = new List<OffensiveWord>
            {
                new OffensiveWord { Word = "badword" },
                new OffensiveWord { Word = "worseword" }
            };

            this.mockOffensiveWordsDbSet = this.offensiveWordsInDatabase.AsQueryable().BuildMockDbSet();

            this.mockDatabaseContext
                .Setup(context => context.OffensiveWords)
                .Returns(this.mockOffensiveWordsDbSet.Object);

            this.offensiveWordsRepository = new OffensiveWordsRepository(this.mockDatabaseContext.Object);
        }

        [Fact]
        public async Task DeleteWord_WhenWordExists_RemovesWordAndSavesChanges()
        {
            // Arrange
            string existingWord = "badword";
            OffensiveWord? removedWord = null;

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Remove(It.IsAny<OffensiveWord>()))
                .Callback<OffensiveWord>(word => removedWord = word);

            this.mockDatabaseContext
                .Setup(context => context.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await this.offensiveWordsRepository.DeleteWord(existingWord);

            // Assert
            Assert.NotNull(removedWord);
            Assert.Equal(existingWord, removedWord.Word);
        }

        [Fact]
        public async Task DeleteWord_WhenWordIsNullOrWhitespace_DoesNotCallRemove()
        {
            // Arrange
            bool removeCalled = false;
            string whitespaceWord = "   ";

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Remove(It.IsAny<OffensiveWord>()))
                .Callback(() => removeCalled = true);

            // Act
            await this.offensiveWordsRepository.DeleteWord(whitespaceWord);

            // Assert
            Assert.False(removeCalled);
        }

        [Fact]
        public async Task DeleteWord_WhenWordDoesNotExist_DoesNotCallRemove()
        {
            // Arrange
            bool removeCalled = false;
            string nonExistentWord = "nonexistent";

            this.mockOffensiveWordsDbSet
                .Setup(set => set.Remove(It.IsAny<OffensiveWord>()))
                .Callback(() => removeCalled = true);

            // Act
            await this.offensiveWordsRepository.DeleteWord(nonExistentWord);

            // Assert
            Assert.False(removeCalled);
        }
    }
}