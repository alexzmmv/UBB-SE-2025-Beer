using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.DrinkModificationRequestServiceTests
{
    public class DrinkModificationRequestServiceGetModificationRequestTest
    {
        private readonly Mock<IDrinkModificationRequestRepository> mockDrinkModificationRequestRepository;
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkModificationRequestService service;

        private const int ExistingRequestId = 1;
        private const int NonExistingRequestId = 999;

        public DrinkModificationRequestServiceGetModificationRequestTest()
        {
            this.mockDrinkModificationRequestRepository = new Mock<IDrinkModificationRequestRepository>();
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.service = new DrinkModificationRequestService(
                this.mockDrinkModificationRequestRepository.Object,
                this.mockDrinkRepository.Object);

            // Setup for existing request
            this.mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(ExistingRequestId))
                .ReturnsAsync(new DrinkModificationRequestDTO
                {
                    DrinkModificationRequestId = ExistingRequestId,
                    ModificationType = DataAccess.Constants.DrinkModificationRequestType.Edit,
                    OldDrinkId = 10,
                    NewDrinkId = 20,
                    RequestingUserId = Guid.NewGuid()
                });

            // Setup for non-existing request to throw or return null (depending on implementation)
            this.mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(NonExistingRequestId))
                .ReturnsAsync((DrinkModificationRequestDTO?)null);
        }

        [Fact]
        public async Task GetModificationRequest_WithExistingId_ReturnsRequest()
        {
            // Act
            DrinkModificationRequestDTO? result = await this.service.GetModificationRequest(ExistingRequestId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ExistingRequestId, result.DrinkModificationRequestId);
            this.mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(ExistingRequestId), Times.Once);
        }

        [Fact]
        public async Task GetModificationRequest_WithNonExistingId_ReturnsNull()
        {
            // Act
            DrinkModificationRequestDTO? result = await this.service.GetModificationRequest(NonExistingRequestId);

            // Assert
            Assert.Null(result);
            this.mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(NonExistingRequestId), Times.Once);
        }
    }
}
