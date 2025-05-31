using Moq;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    //public class ReviewServiceDeleteReviewTest
    //{
    //    private readonly ReviewService service;
    //    private readonly Mock<IReviewRepository> repository;

    //    public ReviewServiceDeleteReviewTest()
    //    {
    //        repository = new Mock<IReviewRepository>();
    //        service = new ReviewService(repository.Object);
    //    }

    //    [Fact]
    //    public void DeleteReviewById_WhenCalled_InvokesRepositoryDeleteOnce()
    //    {
    //        // Arrange
    //        var reviewId = 123;

    //        // Act
    //        service.DeleteReviewById(reviewId);

    //        // Assert
    //        repository.Verify(repository => repository.DeleteReview(reviewId), Times.Once);
    //    }
    //}
}
