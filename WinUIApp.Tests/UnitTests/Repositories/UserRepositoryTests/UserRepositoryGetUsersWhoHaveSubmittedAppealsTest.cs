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
            users = new List<User>();
            dbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_Success_ReturnsUsers()
        {
            // Arrange
            var user1 = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = true };
            var user2 = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = false };
            users.AddRange(new[] { user1, user2 });

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersWhoHaveSubmittedAppeals();

            // Assert
            Assert.Single(result);
            Assert.Equal(user1.UserId, result[0].UserId);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), HasSubmittedAppeal = false };
            users.Add(user);

            var localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            dbContextMock.Setup(x => x.Users).Returns(localDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersWhoHaveSubmittedAppeals();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersWhoHaveSubmittedAppeals_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetUsersWhoHaveSubmittedAppeals());
        }
    }
} 