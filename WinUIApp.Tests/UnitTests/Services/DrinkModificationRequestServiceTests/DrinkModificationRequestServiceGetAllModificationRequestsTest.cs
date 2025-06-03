using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.DrinkModificationRequestServiceTests
{
    public class DrinkModificationRequestServiceGetAllModificationRequestsTest
    {
        private readonly Mock<IDrinkModificationRequestRepository> mockDrinkModificationRequestRepository;
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkModificationRequestService service;

        public DrinkModificationRequestServiceGetAllModificationRequestsTest()
        {
            this.mockDrinkModificationRequestRepository = new Mock<IDrinkModificationRequestRepository>();
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.service = new DrinkModificationRequestService(
                this.mockDrinkModificationRequestRepository.Object,
                this.mockDrinkRepository.Object);
        }

        [Fact]
        public async Task GetAllModificationRequests_ReturnsAllRequestsFromRepository()
        {
            // Arrange
            var mockRequests = new List<DrinkModificationRequestDTO>
            {
                new DrinkModificationRequestDTO { DrinkModificationRequestId = 1 },
                new DrinkModificationRequestDTO { DrinkModificationRequestId = 2 }
            };

            this.mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetAllModificationRequests())
                .ReturnsAsync(mockRequests);

            // Act
            IEnumerable<DrinkModificationRequestDTO> result = await this.service.GetAllModificationRequests();

            // Assert
            Assert.Equal(mockRequests, result);
            this.mockDrinkModificationRequestRepository.Verify(repo => repo.GetAllModificationRequests(), Times.Once);
        }
    }
}