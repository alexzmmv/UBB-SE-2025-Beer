using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceAddUpgradeRequestTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService upgradeRequestsService;

        private readonly Guid EXISTING_USER_ID = Guid.NewGuid();
        private readonly Guid FAILING_USER_ID = Guid.NewGuid();

        public UpgradeRequestsServiceAddUpgradeRequestTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.upgradeRequestsService = new UpgradeRequestsService(
                this.mockUpgradeRequestsRepository.Object,
                this.mockRolesRepository.Object,
                this.mockUserRepository.Object
            );

            // Setup successful add
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.AddUpgradeRequest(EXISTING_USER_ID))
                .Returns(Task.CompletedTask);

            // Setup failure scenario
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.AddUpgradeRequest(FAILING_USER_ID))
                .ThrowsAsync(new Exception("Simulated failure"));
        }

        [Fact]
        public async Task AddUpgradeRequest_WhenCalledWithValidUserId_CallsRepository()
        {
            // Act
            await this.upgradeRequestsService.AddUpgradeRequest(EXISTING_USER_ID);

            // Assert
            this.mockUpgradeRequestsRepository.Verify(repo => repo.AddUpgradeRequest(EXISTING_USER_ID), Times.Once);
        }

        [Fact]
        public async Task AddUpgradeRequest_WhenRepositoryThrows_DoesNotThrow()
        {
            // Act
            var exception = await Record.ExceptionAsync(() => this.upgradeRequestsService.AddUpgradeRequest(FAILING_USER_ID));

            // Assert
            Assert.Null(exception); // Exception should be swallowed by the service
            this.mockUpgradeRequestsRepository.Verify(repo => repo.AddUpgradeRequest(FAILING_USER_ID), Times.Once);
        }
    }
}