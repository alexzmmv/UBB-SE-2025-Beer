using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Repositories;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories
{
    //public class ReviewRepositoryGetAllTest
    //{
    //    private readonly List<Review> reviewList;
    //    private readonly Mock<DbSet<Review>> mockReviewSet;
    //    private readonly Mock<IAppDbContext> mockDbContext;
    //    private readonly ReviewRepository repository;

    //    public ReviewRepositoryGetAllTest()
    //    {
    //        reviewList = new List<Review>
    //        {
    //            new Review
    //            {
    //                ReviewId = 1,
    //                RatingId = 10,
    //                UserId = 1,
    //                Content = "Great",
    //                Rating = new Rating { RatingId = 10 }
    //            },
    //            new Review
    //            {
    //                ReviewId = 2,
    //                RatingId = 20,
    //                UserId = 2,
    //                Content = "Bad",
    //                Rating = new Rating { RatingId = 20 }
    //            },
    //            new Review
    //            {
    //                ReviewId = 3,
    //                RatingId = 10,
    //                UserId = 3,
    //                Content = "Average",
    //                Rating = new Rating { RatingId = 10 }
    //            }
    //        };

    //        IQueryable<Review> queryableReviews = reviewList.AsQueryable();

    //        mockReviewSet = new Mock<DbSet<Review>>();
    //        mockReviewSet.As<IQueryable<Review>>().Setup(setup => setup.Provider).Returns(queryableReviews.Provider);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(setup => setup.Expression).Returns(queryableReviews.Expression);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(setup => setup.ElementType).Returns(queryableReviews.ElementType);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(setup => setup.GetEnumerator()).Returns(() => queryableReviews.GetEnumerator());

    //        mockDbContext = new Mock<IAppDbContext>();
    //        mockDbContext.Setup(database => database.Reviews).Returns(mockReviewSet.Object);

    //        repository = new ReviewRepository(mockDbContext.Object);
    //    }

    //    [Fact]
    //    public void GetReviewById_Returns_Review_If_Exists()
    //    {
    //        Review? result = repository.GetReviewById(1);

    //        Assert.NotNull(result);
    //        Assert.Equal(1, result.ReviewId);
    //    }

    //    [Fact]
    //    public void GetReviewById_Returns_Null_If_Not_Found()
    //    {
    //        Review? result = repository.GetReviewById(99);

    //        Assert.Null(result);
    //    }

    //    [Fact]
    //    public void GetAllReviews_Returns_All_Reviews()
    //    {
    //        List<Review> result = repository.GetAllReviews();

    //        Assert.Equal(3, result.Count);
    //    }

    //    [Fact]
    //    public void GetReviewsByRatingId_Returns_Matching_Reviews()
    //    {
    //        List<Review> result = repository.GetReviewsByRatingId(10);

    //        Assert.Equal(2, result.Count);
    //        Assert.All(result, review => Assert.Equal(10, review.Rating.RatingId));
    //    }

    //    [Fact]
    //    public void CheckIfReviewExists_Returns_True_If_Exists()
    //    {
    //        bool exists = repository.CheckIfReviewExists(1);

    //        Assert.True(exists);
    //    }

    //    [Fact]
    //    public void CheckIfReviewExists_Returns_False_If_Not_Found()
    //    {
    //        bool exists = repository.CheckIfReviewExists(999);

    //        Assert.False(exists);
    //    }
    //}
}
