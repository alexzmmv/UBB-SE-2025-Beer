using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUserByUsernameTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetUserByUsernameTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetUserByUsername_Success_ReturnsUser()
        {
            // Arrange
            string username = "testUser";
            User user = new User { UserId = Guid.NewGuid(), Username = username };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            User? result = await this.userRepository.GetUserByUsername(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task GetUserByUsername_NonExistentUser_ReturnsNull()
        {
            // Arrange
            string username = "nonexistent";
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            User? result = await this.userRepository.GetUserByUsername(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUsername_DbSetThrowsException_Throws()
        {
            // Arrange
            string username = "testUser";
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetUserByUsername(username));
        }
    }
} 