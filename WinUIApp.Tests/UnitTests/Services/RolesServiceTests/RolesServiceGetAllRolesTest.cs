using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.RolesServiceTests
{
    public class RolesServiceGetAllRolesTest
    {
        private readonly Mock<IRolesRepository> rolesRepositoryMock;
        private readonly RolesService rolesService;

        public RolesServiceGetAllRolesTest()
        {
            this.rolesRepositoryMock = new Mock<IRolesRepository>();
            this.rolesService = new RolesService(this.rolesRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllRolesAsync_Success_ReturnsRolesList()
        {
            // Arrange
            List<Role> expectedRoles = new List<Role>
            {
                new Role(RoleType.Banned, "Banned"),
                new Role(RoleType.User, "User"),
                new Role(RoleType.Admin, "Admin")
            };

            this.rolesRepositoryMock.Setup(rolesRepository => rolesRepository.GetAllRoles())
                .ReturnsAsync(expectedRoles);

            // Act
            List<Role> result = await this.rolesService.GetAllRolesAsync();

            // Assert
            Assert.Equal(expectedRoles.Count, result.Count);
            Assert.Equal(expectedRoles[0].RoleType, result[0].RoleType);
            Assert.Equal(expectedRoles[1].RoleType, result[1].RoleType);
            Assert.Equal(expectedRoles[2].RoleType, result[2].RoleType);
        }

        [Fact]
        public async Task GetAllRolesAsync_Exception_ReturnsEmptyList()
        {
            // Arrange
            this.rolesRepositoryMock.Setup(rolesRepository => rolesRepository.GetAllRoles())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            List<Role> result = await this.rolesService.GetAllRolesAsync();

            // Assert
            Assert.Empty(result);
        }
    }
} 