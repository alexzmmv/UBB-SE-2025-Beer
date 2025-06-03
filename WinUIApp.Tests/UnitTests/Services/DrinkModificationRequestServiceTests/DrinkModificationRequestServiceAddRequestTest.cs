using DataAccess.Constants;
using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.DrinkModificationRequestServiceTests
{
    public class DrinkModificationRequestServiceAddRequestTest
    {
        private readonly Mock<IDrinkModificationRequestRepository> mockDrinkModificationRequestRepository;
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkModificationRequestService service;

        private readonly Guid testUserId = Guid.NewGuid();

        public DrinkModificationRequestServiceAddRequestTest()
        {
            this.mockDrinkModificationRequestRepository = new Mock<IDrinkModificationRequestRepository>();
            this.mockDrinkRepository = new Mock<IDrinkRepository>();
            this.service = new DrinkModificationRequestService(
                this.mockDrinkModificationRequestRepository.Object,
                this.mockDrinkRepository.Object);

            this.mockDrinkModificationRequestRepository
                .Setup(repo => repo.AddRequest(It.IsAny<DrinkModificationRequestDTO>()))
                .Returns((DrinkModificationRequestDTO req) => req);
        }

        [Fact]
        public void AddRequest_ValidInput_CreatesAndReturnsRequest()
        {
            // Arrange
            var type = DrinkModificationRequestType.Edit;
            int? oldDrinkId = 1;
            int? newDrinkId = 2;

            // Act
            DrinkModificationRequestDTO result = this.service.AddRequest(type, oldDrinkId, newDrinkId, testUserId);

            // Assert
            this.mockDrinkModificationRequestRepository.Verify(
                repo => repo.AddRequest(It.Is<DrinkModificationRequestDTO>(
                    r => r.ModificationType == type &&
                         r.OldDrinkId == oldDrinkId &&
                         r.NewDrinkId == newDrinkId &&
                         r.RequestingUserId == testUserId)),
                Times.Once);

            Assert.Equal(type, result.ModificationType);
            Assert.Equal(oldDrinkId, result.OldDrinkId);
            Assert.Equal(newDrinkId, result.NewDrinkId);
            Assert.Equal(testUserId, result.RequestingUserId);
        }
    }
}