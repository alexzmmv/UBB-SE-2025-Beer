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
            var userId = Guid.NewGuid();
            var expectedRoleType = RoleType.Admin;

            userRepositoryMock.Setup(x => x.GetRoleTypeForUser(userId))
                .ReturnsAsync(expectedRoleType);

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Equal(expectedRoleType, result);
            userRepositoryMock.Verify(x => x.GetRoleTypeForUser(userId), Times.Once);
        }

        [Fact]
        public async Task GetHighestRoleTypeForUser_UserNotFound_ReturnsBannedRole()
        {
            // Arrange
            var userId = Guid.NewGuid();

            userRepositoryMock.Setup(x => x.GetRoleTypeForUser(userId))
                .ReturnsAsync((RoleType?)null);

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
            userRepositoryMock.Verify(x => x.GetRoleTypeForUser(userId), Times.Once);
        }

        [Fact]
        public async Task GetHighestRoleTypeForUser_Exception_ReturnsBannedRole()
        {
            // Arrange
            var userId = Guid.NewGuid();

            userRepositoryMock.Setup(x => x.GetRoleTypeForUser(userId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await userService.GetHighestRoleTypeForUser(userId);

            // Assert
            Assert.Null(result);
            userRepositoryMock.Verify(x => x.GetRoleTypeForUser(userId), Times.Once);
        }
    }
} 