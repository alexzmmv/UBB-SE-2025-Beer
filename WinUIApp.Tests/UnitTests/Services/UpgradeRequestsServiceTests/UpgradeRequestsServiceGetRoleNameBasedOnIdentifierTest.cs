using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceGetRoleNameBasedOnIdentifierTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService service;

        public UpgradeRequestsServiceGetRoleNameBasedOnIdentifierTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.service = new UpgradeRequestsService(
                mockUpgradeRequestsRepository.Object,
                mockRolesRepository.Object,
                mockUserRepository.Object
            );
        }

        [Fact]
        public async Task GetRoleNameBasedOnIdentifier_WhenRoleExists_ReturnsCorrectName()
        {
            // Arrange
            var roleType = RoleType.Banned;
            var expectedRoleName = "Banned";

            var roles = new List<Role>
            {
                new Role { RoleType = RoleType.User, RoleName = "User" },
                new Role { RoleType = RoleType.Banned, RoleName = "Banned" },
                new Role { RoleType = RoleType.Admin, RoleName = "Admin" }
            };

            mockRolesRepository
                .Setup(repo => repo.GetAllRoles())
                .ReturnsAsync(roles);

            // Act
            var result = await service.GetRoleNameBasedOnIdentifier(roleType);

            // Assert
            Assert.Equal(expectedRoleName, result);
            mockRolesRepository.Verify(repo => repo.GetAllRoles(), Times.Once);
        }

        [Fact]
        public async Task GetRoleNameBasedOnIdentifier_WhenRoleDoesNotExist_ReturnsEmptyString()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { RoleType = RoleType.User, RoleName = "User" }
            };

            mockRolesRepository
                .Setup(repo => repo.GetAllRoles())
                .ReturnsAsync(roles);

            // Act
            var result = await service.GetRoleNameBasedOnIdentifier(RoleType.Admin);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task GetRoleNameBasedOnIdentifier_WhenExceptionThrown_ReturnsEmptyString()
        {
            // Arrange
            mockRolesRepository
                .Setup(repo => repo.GetAllRoles())
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            var result = await service.GetRoleNameBasedOnIdentifier(RoleType.User);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}