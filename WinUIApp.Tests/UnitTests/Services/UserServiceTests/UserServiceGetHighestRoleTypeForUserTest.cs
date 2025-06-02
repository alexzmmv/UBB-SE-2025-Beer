using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetHighestRoleTypeForUserTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetHighestRoleTypeForUserTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetHighestRoleTypeForUser_Success_ReturnsRoleType()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            RoleType expectedRoleType = RoleType.Admin;

            this.userRepositoryMock.Setup(userRepository => userRepository.GetRoleTypeForUser(userId))
                .ReturnsAsync(expectedRoleType);

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Equal(expectedRoleType, result);
            this.userRepositoryMock.Verify(userRepository => userRepository.GetRoleTypeForUser(userId), Times.Once);
        }

        [Fact]
        public async Task GetHighestRoleTypeForUser_UserNotFound_ReturnsBannedRole()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            this.userRepositoryMock.Setup(userRepository => userRepository.GetRoleTypeForUser(userId))
                .ReturnsAsync((RoleType?)null);

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.GetRoleTypeForUser(userId), Times.Once);
        }

        [Fact]
        public async Task GetHighestRoleTypeForUser_Exception_ReturnsBannedRole()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            this.userRepositoryMock.Setup(userRepository => userRepository.GetRoleTypeForUser(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
            this.userRepositoryMock.Verify(userRepository => userRepository.GetRoleTypeForUser(userId), Times.Once);
        }
    }
} 