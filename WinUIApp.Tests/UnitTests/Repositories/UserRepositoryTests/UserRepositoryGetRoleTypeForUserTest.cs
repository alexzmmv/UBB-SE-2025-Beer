using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetRoleTypeForUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;

        public UserRepositoryGetRoleTypeForUserTest()
        {
            this.users = new List<User>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task GetRoleTypeForUser_Success_ReturnsRoleType()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            User user = new User { UserId = userId, AssignedRole = RoleType.Admin };
            this.users.Add(user);

            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            RoleType? result = await this.userRepository.GetRoleTypeForUser(userId);

            // Assert
            Assert.Equal(RoleType.Admin, result);
        }

        [Fact]
        public async Task GetRoleTypeForUser_UserNotFound_ReturnsNull()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(new List<User>());
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);

            // Act
            RoleType? result = await this.userRepository.GetRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRoleTypeForUser_DbSetThrowsException_Throws()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetRoleTypeForUser(userId));
        }
    }
} 