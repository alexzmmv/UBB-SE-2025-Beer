using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryLoadOffensiveWordsTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<OffensiveWord>> dbSetMock;
        private readonly OffensiveWordsRepository offensiveWordsRepository;
        private readonly List<OffensiveWord> offensiveWords;

        public OffensiveWordsRepositoryLoadOffensiveWordsTest()
        {
            offensiveWords = new List<OffensiveWord>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.OffensiveWords).Returns(dbSetMock.Object);
            offensiveWordsRepository = new OffensiveWordsRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task LoadOffensiveWords_Success_ReturnsHashSet()
        {
            // Arrange
            var offensiveWord1 = new OffensiveWord { Word = "badword1" };
            var offensiveWord2 = new OffensiveWord { Word = "badword2" };
            offensiveWords.Add(offensiveWord1);
            offensiveWords.Add(offensiveWord2);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            var result = await offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task LoadOffensiveWords_Success_ReturnsCorrectCount()
        {
            // Arrange
            var offensiveWord1 = new OffensiveWord { Word = "badword1" };
            var offensiveWord2 = new OffensiveWord { Word = "badword2" };
            offensiveWords.Add(offensiveWord1);
            offensiveWords.Add(offensiveWord2);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            var result = await offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task LoadOffensiveWords_Success_ContainsExpectedWords()
        {
            // Arrange
            var offensiveWord1 = new OffensiveWord { Word = "badword1" };
            var offensiveWord2 = new OffensiveWord { Word = "badword2" };
            offensiveWords.Add(offensiveWord1);
            offensiveWords.Add(offensiveWord2);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            var result = await offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            Assert.Contains("badword1", result);
        }

        [Fact]
        public async Task LoadOffensiveWords_Success_IsCaseInsensitive()
        {
            // Arrange
            var offensiveWord1 = new OffensiveWord { Word = "BadWord1" };
            offensiveWords.Add(offensiveWord1);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            var result = await offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            Assert.Contains("badword1", result);
        }

        [Fact]
        public async Task LoadOffensiveWords_EmptyDatabase_ReturnsEmptyHashSet()
        {
            // Arrange
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            var result = await offensiveWordsRepository.LoadOffensiveWords();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadOffensiveWords_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.OffensiveWords).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => offensiveWordsRepository.LoadOffensiveWords());
        }
    }
} 