using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceUpdateUserRoleTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceUpdateUserRoleTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateUserRole_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            userRepositoryMock.Verify(x => x.GetUserById(userId), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUser(It.Is<User>(u => 
                u.UserId == userId && 
                u.AssignedRole == newRole)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_UserNotFound_NoUpdate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;

            userRepositoryMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync((User?)null);

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            userRepositoryMock.Verify(x => x.GetUserById(userId), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserRole_UpdateFails_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            userRepositoryMock.Verify(x => x.GetUserById(userId), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUser(It.Is<User>(u => 
                u.UserId == userId && 
                u.AssignedRole == newRole)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_Exception_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            userRepositoryMock.Verify(x => x.GetUserById(userId), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUser(It.Is<User>(u => 
                u.UserId == userId && 
                u.AssignedRole == newRole)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_GetUserException_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;

            userRepositoryMock.Setup(x => x.GetUserById(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            userRepositoryMock.Verify(x => x.GetUserById(userId), Times.Once);
            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Never);
        }
    }
} 