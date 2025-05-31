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
    //public class ReviewRepositoryDeleteReviewTest
    //{
    //    private readonly List<Review> reviewList;
    //    private readonly Mock<DbSet<Review>> mockReviewSet;
    //    private readonly Mock<IAppDbContext> mockDbContext;
    //    private readonly ReviewRepository repository;

    //    public ReviewRepositoryDeleteReviewTest()
    //    {
    //        reviewList = new List<Review>
    //        {
    //            new Review { ReviewId = 1, Content = "Sample", RatingId = 1, UserId = 1 }
    //        };

    //        IQueryable<Review> queryableReviews = reviewList.AsQueryable();

    //        mockReviewSet = new Mock<DbSet<Review>>();
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.Provider).Returns(queryableReviews.Provider);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.Expression).Returns(queryableReviews.Expression);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.ElementType).Returns(queryableReviews.ElementType);
    //        mockReviewSet.As<IQueryable<Review>>().Setup(set => set.GetEnumerator()).Returns(() => queryableReviews.GetEnumerator());

    //        mockReviewSet.Setup(set => set.Remove(It.IsAny<Review>()))
    //                     .Callback<Review>(review => reviewList.Remove(review));

    //        mockDbContext = new Mock<IAppDbContext>();
    //        mockDbContext.Setup(database => database.Reviews).Returns(mockReviewSet.Object);
    //        mockDbContext.Setup(database => database.SaveChanges()).Returns(1);

    //        repository = new ReviewRepository(mockDbContext.Object);
    //    }

    //    [Fact]
    //    public void DeleteReview_Removes_Review_From_Collection()
    //    {
    //        int reviewIdToDelete = 1;

    //        repository.DeleteReview(reviewIdToDelete);

    //        Assert.Empty(reviewList);
    //    }

    //    [Fact]
    //    public void DeleteReview_Calls_SaveChanges_Once()
    //    {
    //        int reviewIdToDelete = 1;

    //        repository.DeleteReview(reviewIdToDelete);

    //        mockDbContext.Verify(database => database.SaveChanges(), Times.Once);
    //    }

    //    [Fact]
    //    public void DeleteReview_Throws_Exception_When_Review_Not_Found()
    //    {
    //        int nonexistentReviewId = 99;

    //        Exception exception = Assert.Throws<Exception>(() => repository.DeleteReview(nonexistentReviewId));

    //        Assert.Equal(RepositoryErrorMessages.EntityNotFound, exception.Message);
    //    }
    //}
}
