using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.OffensiveWordsRepositoryTests
{
    public class OffensiveWordsRepositoryDeleteWordTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<OffensiveWord>> dbSetMock;
        private readonly OffensiveWordsRepository offensiveWordsRepository;
        private readonly List<OffensiveWord> offensiveWords;

        public OffensiveWordsRepositoryDeleteWordTest()
        {
            offensiveWords = new List<OffensiveWord>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.OffensiveWords).Returns(dbSetMock.Object);
            offensiveWordsRepository = new OffensiveWordsRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task DeleteWord_Success_RemovesWordFromDatabase()
        {
            // Arrange
            var word = "testword";
            var existingWord = new OffensiveWord { Word = word };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Remove(It.Is<OffensiveWord>(o => o.Word == word)), Times.Once);
        }

        [Fact]
        public async Task DeleteWord_Success_CallsSaveChanges()
        {
            // Arrange
            var word = "testword";
            var existingWord = new OffensiveWord { Word = word };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteWord_WordDoesNotExist_DoesNotRemove()
        {
            // Arrange
            var word = "nonexistentword";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Remove(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWord_WordDoesNotExist_DoesNotCallSaveChanges()
        {
            // Arrange
            var word = "nonexistentword";
            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<OffensiveWord>());
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            dbContextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteWord_NullWord_DoesNotRemove()
        {
            // Arrange
            string? word = null;
            var existingWord = new OffensiveWord { Word = "existingword" };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Remove(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWord_EmptyWord_DoesNotRemove()
        {
            // Arrange
            var word = string.Empty;
            var existingWord = new OffensiveWord { Word = "existingword" };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Remove(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWord_WhitespaceWord_DoesNotRemove()
        {
            // Arrange
            var word = "   ";
            var existingWord = new OffensiveWord { Word = "existingword" };
            offensiveWords.Add(existingWord);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(offensiveWords);
            dbContextMock.Setup(x => x.OffensiveWords).Returns(localDbSetMock.Object);

            // Act
            await offensiveWordsRepository.DeleteWord(word);

            // Assert
            localDbSetMock.Verify(x => x.Remove(It.IsAny<OffensiveWord>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWord_DbSetThrowsException_Throws()
        {
            // Arrange
            var word = "testword";
            dbContextMock.Setup(x => x.OffensiveWords).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => offensiveWordsRepository.DeleteWord(word));
        }
    }
} 