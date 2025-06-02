using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceChangeRoleToUserTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;
        private readonly Role userRole;

        public UserServiceChangeRoleToUserTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
            userRole = new Role(RoleType.User, "User");
        }

        [Fact]
        public async Task ChangeRoleToUser_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            await userService.ChangeRoleToUser(userId, userRole);

            // Assert
            userRepositoryMock.Verify(x => x.ChangeRoleToUser(userId, userRole), Times.Once);
        }

        [Fact]
        public async Task ChangeRoleToUser_RepositoryThrowsException_HandlesGracefully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            userRepositoryMock.Setup(x => x.ChangeRoleToUser(userId, userRole))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await userService.ChangeRoleToUser(userId, userRole);
            userRepositoryMock.Verify(x => x.ChangeRoleToUser(userId, userRole), Times.Once);
        }
    }
} 