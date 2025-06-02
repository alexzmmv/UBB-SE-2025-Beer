using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryAddWordTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<OffensiveWord>> dbSetMock;
        private readonly OffensiveWordsRepository offensiveWordsRepository;
        private readonly List<OffensiveWord> offensiveWords;

        public OffensiveWordsRepositoryAddWordTest()
        {
            offensiveWords = new List<OffensiveWord>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.OffensiveWords).Returns(dbSetMock.Object);
            offensiveWordsRepository = new OffensiveWordsRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task AddWord_Success_AddsWordToDatabase()
        {
            // Arrange
            var word = "testword";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Add(It.Is<OffensiveWord>(o => o.Word == word)), Times.Once);
        }

        [Fact]
        public async Task AddWord_Success_CallsSaveChanges()
        {
            // Arrange
            var word = "testword";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddWord_WordAlreadyExists_DoesNotAddDuplicate()
        {
            // Arrange
            var word = "existingword";
            var existingWord = new OffensiveWord { Word = word };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Add(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task AddWord_WordAlreadyExists_DoesNotCallSaveChanges()
        {
            // Arrange
            var word = "existingword";
            var existingWord = new OffensiveWord { Word = word };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddWord_NullWord_DoesNotAddToDatabase()
        {
            // Arrange
            string? word = null;
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Add(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task AddWord_EmptyWord_DoesNotAddToDatabase()
        {
            // Arrange
            var word = string.Empty;
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Add(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task AddWord_WhitespaceWord_DoesNotAddToDatabase()
        {
            // Arrange
            var word = "   ";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.AddWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Add(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task AddWord_DbSetThrowsException_Throws()
        {
            // Arrange
            var word = "testword";
            dbContextMock.Setup(x => x.OffensiveWords).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => offensiveWordsRepository.AddWord(word));
        }
    }
} 