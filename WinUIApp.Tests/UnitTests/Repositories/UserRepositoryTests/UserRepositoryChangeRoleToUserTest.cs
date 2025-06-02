using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryChangeRoleToUserTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> dbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;
        private readonly Mock<DbSet<Role>> rolesDbSetMock;
        private readonly List<Role> roles;

        public UserRepositoryChangeRoleToUserTest()
        {
            this.users = new List<User>();
            this.roles = new List<Role>();
            this.dbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.rolesDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.roles);
            this.dbContextMock = new Mock<IAppDbContext>();
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Roles).Returns(this.rolesDbSetMock.Object);
            this.userRepository = new UserRepository(this.dbContextMock.Object);
        }

        [Fact]
        public async Task ChangeRoleToUser_UserExists_UpdatesRole()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Role roleToAdd = new Role(RoleType.Admin, "Admin");
            User user = new User { UserId = userId, AssignedRole = RoleType.User };
            this.users.Add(user);
            Mock<DbSet<User>> localDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users.Update(It.IsAny<User>())).Callback<User>(userEntity => {
                int userIndex = this.users.FindIndex(userListItem => userListItem.UserId == userEntity.UserId);
                if (userIndex >= 0) this.users[userIndex] = userEntity;
            });
            this.dbContextMock.Setup(databaseContext => databaseContext.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await this.userRepository.ChangeRoleToUser(userId, roleToAdd);

            // Assert
            Assert.Equal(RoleType.Admin, this.users[0].AssignedRole);
            this.dbContextMock.Verify(databaseContext => databaseContext.Users.Update(It.Is<User>(userEntity => userEntity.UserId == userId && userEntity.AssignedRole == RoleType.Admin)), Times.Once);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangeRoleToUser_UserNotFound_DoesNothing()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Role roleToAdd = new Role(RoleType.Admin, "Admin");
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(this.dbSetMock.Object);

            // Act
            await this.userRepository.ChangeRoleToUser(userId, roleToAdd);

            // Assert
            this.dbContextMock.Verify(databaseContext => databaseContext.Users.Update(It.IsAny<User>()), Times.Never);
            this.dbContextMock.Verify(databaseContext => databaseContext.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ChangeRoleToUser_DbSetThrowsException_Throws()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Role roleToAdd = new Role(RoleType.Admin, "Admin");
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.ChangeRoleToUser(userId, roleToAdd));
        }
    }
} 