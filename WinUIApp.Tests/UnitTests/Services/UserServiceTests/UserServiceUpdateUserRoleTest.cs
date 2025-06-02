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
            Guid userId = Guid.NewGuid();
            RoleType newRole = RoleType.Admin;
            User user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync(user);
            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await this.userService.UpdateUserRole(userId, newRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.GetUserById(userId), Times.Once);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.AssignedRole == newRole)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_UserNotFound_NoUpdate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync((User?)null);

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.GetUserById(userId), Times.Once);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.IsAny<User>()), Times.Never);
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

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync(user);
            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.GetUserById(userId), Times.Once);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.AssignedRole == newRole)), 
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

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ReturnsAsync(user);
            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.GetUserById(userId), Times.Once);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.AssignedRole == newRole)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserRole_GetUserException_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newRole = RoleType.Admin;

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUserById(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await userService.UpdateUserRole(userId, newRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.GetUserById(userId), Times.Once);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.IsAny<User>()), Times.Never);
        }
    }
} 