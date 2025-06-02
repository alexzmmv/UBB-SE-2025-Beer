using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUserByIdTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetUserByIdTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetUserById_Success_ReturnsUser()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, Username = "testUser" };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            User? result = await this.userRepository.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetUserById_NonExistentUser_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            User? result = await this.userRepository.GetUserById(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserById_DbSetThrowsException_Throws()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetUserById(userId));
        }
    }
} 