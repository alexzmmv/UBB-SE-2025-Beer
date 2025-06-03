using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceProcessUpgradeRequestTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<UpgradeRequestsService> mockService;

        private readonly UpgradeRequestsService service;

        private const int VALID_REQUEST_ID = 1;
        private const int INVALID_REQUEST_ID = 999;

        public UpgradeRequestsServiceProcessUpgradeRequestTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            // Create a partial mock so we can mock SendEmail
            this.mockService = new Mock<UpgradeRequestsService>(
                mockUpgradeRequestsRepository.Object,
                mockRolesRepository.Object,
                mockUserRepository.Object
            )
            { CallBase = true };

            this.service = mockService.Object;

            mockUpgradeRequestsRepository
                .Setup(r => r.RemoveUpgradeRequestByIdentifier(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            mockService
                .Setup(s => s.SendEmail(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task ProcessUpgradeRequest_WhenAccepted_CallsSendEmailAndRemovesRequest()
        {
            // Act
            await service.ProcessUpgradeRequest(true, VALID_REQUEST_ID);

            // Assert
            mockService.Verify(s => s.SendEmail(VALID_REQUEST_ID), Times.Once);
            mockUpgradeRequestsRepository.Verify(r => r.RemoveUpgradeRequestByIdentifier(VALID_REQUEST_ID), Times.Once);
        }

        [Fact]
        public async Task ProcessUpgradeRequest_WhenRejected_DoesNotCallSendEmail_ButRemovesRequest()
        {
            // Act
            await service.ProcessUpgradeRequest(false, VALID_REQUEST_ID);

            // Assert
            mockService.Verify(s => s.SendEmail(It.IsAny<int>()), Times.Never);
            mockUpgradeRequestsRepository.Verify(r => r.RemoveUpgradeRequestByIdentifier(VALID_REQUEST_ID), Times.Once);
        }

        [Fact]
        public async Task ProcessUpgradeRequest_WhenExceptionOccurs_DoesNotThrow()
        {
            // Arrange: simulate failure on repository
            mockUpgradeRequestsRepository
                .Setup(r => r.RemoveUpgradeRequestByIdentifier(INVALID_REQUEST_ID))
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            var exception = await Record.ExceptionAsync(() =>
                service.ProcessUpgradeRequest(true, INVALID_REQUEST_ID));

            // Assert: method swallows the exception
            Assert.Null(exception);
        }
    }
}