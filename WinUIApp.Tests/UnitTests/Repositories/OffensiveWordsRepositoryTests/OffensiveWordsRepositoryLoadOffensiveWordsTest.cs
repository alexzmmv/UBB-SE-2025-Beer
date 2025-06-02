using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using Moq;
using MockQueryable.Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryLoadOffensiveWordsTest
    {
        private readonly Mock<IAppDbContext> mockDatabaseContext;
        private readonly Mock<DbSet<OffensiveWord>> mockOffensiveWordsDbSet;
        private readonly OffensiveWordsRepository offensiveWordsRepository;

        private readonly List<OffensiveWord> offensiveWordsInDatabase;

        public OffensiveWordsRepositoryLoadOffensiveWordsTest()
        {
            this.mockDatabaseContext = new Mock<IAppDbContext>();

            this.offensiveWordsInDatabase = new List<OffensiveWord>
            {
                new OffensiveWord { Word = "badword" },
                new OffensiveWord { Word = "terribleword" },
                new OffensiveWord { Word = "nastyword" }
            };

            this.mockOffensiveWordsDbSet = this.offensiveWordsInDatabase.AsQueryable().BuildMockDbSet();

            this.mockDatabaseContext
                .Setup(context => context.OffensiveWords)
                .Returns(this.mockOffensiveWordsDbSet.Object);

            this.offensiveWordsRepository = new OffensiveWordsRepository(this.mockDatabaseContext.Object);
        }

        [Fact]
        public async Task LoadOffensiveWords_WhenCalled_ReturnsAllWordsInHashSet()
        {
            // Act
            HashSet<string> result = await this.offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            HashSet<string> expectedWords = new HashSet<string>(this.offensiveWordsInDatabase.Select(word => word.Word), StringComparer.OrdinalIgnoreCase);
            Assert.Equal(expectedWords, result);
        }

        [Fact]
        public async Task LoadOffensiveWords_WhenDatabaseIsEmpty_ReturnsEmptyHashSet()
        {
            // Arrange
            List<OffensiveWord> emptyOffensiveWords = [];
            Mock<DbSet<OffensiveWord>> emptyMockDbSet = emptyOffensiveWords.AsQueryable().BuildMockDbSet();

            this.mockDatabaseContext
                .Setup(context => context.OffensiveWords)
                .Returns(emptyMockDbSet.Object);

            OffensiveWordsRepository repositoryWithEmptyData = new OffensiveWordsRepository(this.mockDatabaseContext.Object);

            // Act
            HashSet<string> result = await repositoryWithEmptyData.LoadOffensiveWords();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadOffensiveWords_HashSetIsCaseInsensitive()
        {
            // Act
            HashSet<string> result = await this.offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            string originalWord = this.offensiveWordsInDatabase[0].Word;
            string wordInDifferentCase = originalWord.ToUpperInvariant();

            Assert.Contains(wordInDifferentCase, result);
        }
    }
}