using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceUpdateUserTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceUpdateUserTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateUser_Success_ReturnsTrue()
        {
            // Arrange
            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            bool result = await this.userService.UpdateUser(user);

            // Assert
            Assert.True(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_NullUser_ReturnsFalse()
        {
            // Act
            bool result = await this.userService.UpdateUser(null);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_UpdateFails_ReturnsFalse()
        {
            // Arrange
            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            bool result = await this.userService.UpdateUser(user);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_Exception_ReturnsFalse()
        {
            // Arrange
            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            bool result = await this.userService.UpdateUser(user);

            // Assert
            Assert.False(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(user), Times.Once);
        }
    }
} 