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
            var currentRole = RoleType.User;
            var expectedNextRole = new Role(RoleType.Admin, "Admin");

            this.rolesRepositoryMock.Setup(x => x.GetNextRoleInHierarchy(currentRole))
                .ReturnsAsync(expectedNextRole);

            // Act
            var result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNextRole.RoleType, result.RoleType);
            Assert.Equal(expectedNextRole.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetNextRoleInHierarchyAsync_AdminRole_ReturnsAdminRole()
        {
            // Arrange
            var currentRole = RoleType.Admin;
            var expectedRole = new Role(RoleType.Admin, "Admin");

            this.rolesRepositoryMock.Setup(x => x.GetNextRoleInHierarchy(currentRole))
                .ReturnsAsync(expectedRole);

            // Act
            var result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRole.RoleType, result.RoleType);
            Assert.Equal(expectedRole.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetNextRoleInHierarchyAsync_Exception_ReturnsNull()
        {
            // Arrange
            var currentRole = RoleType.User;
            this.rolesRepositoryMock.Setup(x => x.GetNextRoleInHierarchy(currentRole))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.rolesService.GetNextRoleInHierarchyAsync(currentRole);

            // Assert
            Assert.Null(result);
        }
    }
} 