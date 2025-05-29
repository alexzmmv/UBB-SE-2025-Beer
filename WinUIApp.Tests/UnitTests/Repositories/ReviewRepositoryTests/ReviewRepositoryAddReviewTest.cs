using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using WinUIApp.WebAPI.Repositories;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories
{
    public class ReviewRepositoryAddReviewTest
    {
        private readonly List<Review> reviewList;
        private readonly Mock<DbSet<Review>> mockReviewSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly ReviewRepository repository;

        public ReviewRepositoryAddReviewTest()
        {
            reviewList = new List<Review>();

            mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.Setup(set => set.Add(It.IsAny<Review>()))
                         .Callback<Review>(review => reviewList.Add(review));

            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(database => database.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(database => database.SaveChanges()).Returns(1);

            repository = new ReviewRepository(mockDbContext.Object);
        }

        [Fact]
        public void AddReview_Adds_Review_To_Collection()
        {
            Review review = new Review
            {
                RatingId = 1,
                UserId = 1,
                Content = "Great drink!"
            };

            repository.AddReview(review);

            Assert.Single(reviewList);
        }

        [Fact]
        public void AddReview_Sets_CreationDate_When_Null()
        {
            Review review = new Review
            {
                RatingId = 2,
                UserId = 2,
                Content = "Nice.",
                CreationDate = null
            };

            Review result = repository.AddReview(review);

            Assert.NotNull(result.CreationDate);
        }

        [Fact]
        public void AddReview_Sets_IsActive_True_When_Null()
        {
            Review review = new Review
            {
                RatingId = 3,
                UserId = 3,
                Content = "Awesome!",
                IsActive = null
            };

            Review result = repository.AddReview(review);

            Assert.True(result.IsActive.Value);
        }

        [Fact]
        public void AddReview_Throws_When_Content_Is_Null_Or_Whitespace()
        {
            Review review = new Review
            {
                RatingId = 4,
                UserId = 4,
                Content = "  "
            };

            ArgumentException exception = Assert.Throws<ArgumentException>(() => repository.AddReview(review));
            Assert.Equal(nameof(review.Content), exception.ParamName);
        }

        [Fact]
        public void AddReview_Calls_SaveChanges_Once()
        {
            Review review = new Review
            {
                RatingId = 5,
                UserId = 5,
                Content = "Good enough."
            };

            repository.AddReview(review);

            mockDbContext.Verify(database => database.SaveChanges(), Times.Once);
        }
    }
}
