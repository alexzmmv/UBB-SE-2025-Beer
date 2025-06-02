using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.RolesServiceTests
{
    public class RolesServiceGetNextRoleInHierarchyTest
    {
        private readonly Mock<IRolesRepository> rolesRepositoryMock;
        private readonly RolesService rolesService;

        public RolesServiceGetNextRoleInHierarchyTest()
        {
            this.rolesRepositoryMock = new Mock<IRolesRepository>();
            this.rolesService = new RolesService(this.rolesRepositoryMock.Object);
        }

        [Fact]
        public async Task GetNextRoleInHierarchyAsync_Success_ReturnsNextRole()
        {
            // Arrange
            RoleType currentRole = RoleType.User;
            Role expectedNextRole = new Role(RoleType.Admin, "Admin");

            this.rolesRepositoryMock.Setup(rolesRepository => rolesRepository.GetNextRoleInHierarchy(currentRole))
                .ReturnsAsync(expectedNextRole);

            // Act
            Role? result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNextRole.RoleType, result.RoleType);
            Assert.Equal(expectedNextRole.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetNextRoleInHierarchyAsync_AdminRole_ReturnsAdminRole()
        {
            // Arrange
            RoleType currentRole = RoleType.Admin;
            Role expectedRole = new Role(RoleType.Admin, "Admin");

            this.rolesRepositoryMock.Setup(rolesRepository => rolesRepository.GetNextRoleInHierarchy(currentRole))
                .ReturnsAsync(expectedRole);

            // Act
            Role? result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRole.RoleType, result.RoleType);
            Assert.Equal(expectedRole.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetNextRoleInHierarchyAsync_Exception_ReturnsNull()
        {
            // Arrange
            RoleType currentRole = RoleType.User;
            this.rolesRepositoryMock.Setup(rolesRepository => rolesRepository.GetNextRoleInHierarchy(currentRole))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            Role? result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.Null(result);
        }
    }
} 