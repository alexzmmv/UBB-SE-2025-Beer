using DataAccess.Constants;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUsersWhoHaveSubmittedAppealsTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetUsersWhoHaveSubmittedAppealsTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_Success_ReturnsUsers()
        {
            // Arrange
            User user1 = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = true };
            User user2 = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = false };
            this.users.AddRange(new[] { user1, user2 });

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersWhoHaveSubmittedAppeals();

            // Assert
            Assert.Single(result);
            Assert.Equal(user1.UserId, result[0].UserId);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = false };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersWhoHaveSubmittedAppeals();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_DbSetThrowsException_Throws()
        {
            // Arrange
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetUsersWhoHaveSubmittedAppeals());
        }
    }
} 