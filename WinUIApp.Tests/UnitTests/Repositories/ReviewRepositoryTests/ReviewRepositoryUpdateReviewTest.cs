using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using WinUIApp.WebAPI.Repositories;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories
{
    //public class ReviewRepository_UpdateReview_Tests
    //{
    //    private readonly List<Review> reviewList;
    //    private readonly Mock<DbSet<Review>> mockReviewSet;
    //    private readonly Mock<IAppDbContext> mockDbContext;
    //    private readonly ReviewRepository repository;

    //    public ReviewRepository_UpdateReview_Tests()
    //    {
    //        reviewList = new List<Review>
    //        {
    //            new Review
    //            {
    //                ReviewId = 1,
    //                RatingId = 1,
    //                UserId = 1,
    //                Content = "Original Content",
    //                CreationDate = new DateTime(2023, 1, 1),
    //                IsActive = true
    //            }
    //        };

    //        IQueryable<Review> queryableReviews = reviewList.AsQueryable();

    //        mockReviewSet = new Mock<DbSet<Review>>();
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.Provider).Returns(queryableReviews.Provider);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.Expression).Returns(queryableReviews.Expression);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.ElementType).Returns(queryableReviews.ElementType);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.GetEnumerator()).Returns(() => queryableReviews.GetEnumerator());

    //        mockDbContext = new Mock<IAppDbContext>();
    //        mockDbContext.Setup(database => database.Reviews).Returns(mockReviewSet.Object);
    //        mockDbContext.Setup(database => database.SaveChanges()).Returns(1);

    //        repository = new ReviewRepository(mockDbContext.Object);
    //    }

    //    [Fact]
    //    public void UpdateReview_Updates_Fields_Correctly()
    //    {
    //        Review updatedReview = new Review
    //        {
    //            ReviewId = 1,
    //            RatingId = 2,
    //            UserId = 3,
    //            Content = "Updated Content",
    //            CreationDate = new DateTime(2024, 12, 31),
    //            IsActive = false
    //        };

    //        Review result = repository.UpdateReview(updatedReview);

    //        Assert.Equal(2, result.RatingId);
    //        Assert.Equal(3, result.UserId);
    //        Assert.Equal("Updated Content", result.Content);
    //        Assert.Equal(new DateTime(2024, 12, 31), result.CreationDate);
    //        Assert.False(result.IsActive.Value);
    //    }

    //    [Fact]
    //    public void UpdateReview_Keeps_Original_Values_If_Null()
    //    {
    //        Review updatedReview = new Review
    //        {
    //            ReviewId = 1,
    //            RatingId = 5,
    //            UserId = 6,
    //            Content = "Still valid",
    //            CreationDate = null,
    //            IsActive = null
    //        };

    //        Review originalReview = reviewList.First(review => review.ReviewId == 1);
    //        DateTime originalDate = originalReview.CreationDate.Value;
    //        bool originalIsActive = originalReview.IsActive.Value;

    //        Review result = repository.UpdateReview(updatedReview);

    //        Assert.Equal(originalDate, result.CreationDate);
    //        Assert.Equal(originalIsActive, result.IsActive);
    //    }

    //    [Fact]
    //    public void UpdateReview_Throws_If_Review_Not_Found()
    //    {
    //        Review nonExistentReview = new Review
    //        {
    //            ReviewId = 99,
    //            Content = "Missing"
    //        };

    //        Exception exception = Assert.Throws<Exception>(() => repository.UpdateReview(nonExistentReview));

    //        Assert.Equal(RepositoryErrorMessages.EntityNotFound, exception.Message);
    //    }

    //    [Fact]
    //    public void UpdateReview_Throws_If_Content_Is_Empty()
    //    {
    //        Review reviewWithEmptyContent = new Review
    //        {
    //            ReviewId = 1,
    //            Content = "  "
    //        };

    //        ArgumentException exception = Assert.Throws<ArgumentException>(() => repository.UpdateReview(reviewWithEmptyContent));

    //        Assert.Equal(nameof(Review.Content), exception.ParamName);
    //    }

    //    [Fact]
    //    public void UpdateReview_Calls_SaveChanges_Once()
    //    {
    //        Review updatedReview = new Review
    //        {
    //            ReviewId = 1,
    //            Content = "Updated Again",
    //            RatingId = 10,
    //            UserId = 20
    //        };

    //        repository.UpdateReview(updatedReview);

    //        mockDbContext.Verify(database => database.SaveChanges(), Times.Once);
    //    }
    //}
}
