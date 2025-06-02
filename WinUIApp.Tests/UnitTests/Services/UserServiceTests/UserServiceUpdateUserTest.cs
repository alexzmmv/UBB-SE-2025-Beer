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
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await userService.UpdateUser(user);

            // Assert
            Assert.True(result);
            userRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_NullUser_ReturnsFalse()
        {
            // Act
            var result = await userService.UpdateUser(null);

            // Assert
            Assert.False(result);
            userRepositoryMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_UpdateFails_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            var result = await userService.UpdateUser(user);

            // Assert
            Assert.False(result);
            userRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_Exception_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.User
            };

            userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await userService.UpdateUser(user);

            // Assert
            Assert.False(result);
            userRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }
    }
} 