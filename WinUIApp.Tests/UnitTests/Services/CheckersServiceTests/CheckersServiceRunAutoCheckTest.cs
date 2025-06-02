using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    public class CheckersServiceRunAutoCheckTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceRunAutoCheckTest()
        {
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public async Task RunAutoCheck_NullReviews_ReturnsEmptyList()
        {
            // Act
            var result = await checkersService.RunAutoCheck(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_EmptyReviews_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>();

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_ReviewWithNullContent_SkipsReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = null }
            };

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
            autoCheckMock.Verify(x => x.AutoCheckReview(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_HidesReviewAndReturnsMessage()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is offensive. Hiding the review.", result);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_CallsHideReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            await checkersService.RunAutoCheck(reviews);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(1), Times.Once);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_CallsResetReviewFlags()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            await checkersService.RunAutoCheck(reviews);

            // Assert
            reviewServiceMock.Verify(x => x.ResetReviewFlags(1), Times.Once);
        }

        [Fact]
        public async Task RunAutoCheck_NonOffensiveReview_ReturnsNotOffensiveMessage()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Good content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_NonOffensiveReview_DoesNotCallHideReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Good content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);

            // Act
            await checkersService.RunAutoCheck(reviews);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RunAutoCheck_ExceptionThrown_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Test content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Test content")).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_MultipleReviews_ProcessesAllReviews()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Good content" },
                new Review { ReviewId = 2, Content = "Bad content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);
            autoCheckMock.Setup(x => x.AutoCheckReview("Bad content")).ReturnsAsync(true);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task RunAutoCheck_NullReviewInList_SkipsNullReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                null,
                new Review { ReviewId = 2, Content = "Valid content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Valid content")).ReturnsAsync(false);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 2 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_EmptyStringContent_ProcessesReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = string.Empty }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview(string.Empty)).ReturnsAsync(false);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_WhitespaceContent_ProcessesReview()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "   " }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("   ")).ReturnsAsync(false);

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_HideReviewFails_ContinuesProcessing()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);
            reviewServiceMock.Setup(x => x.HideReview(1)).ThrowsAsync(new Exception("Hide failed"));

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_ResetReviewFlagsFails_ContinuesProcessing()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);
            reviewServiceMock.Setup(x => x.ResetReviewFlags(1)).ThrowsAsync(new Exception("Reset failed"));

            // Act
            var result = await checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_NullAutoCheck_ThrowsException()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Content = "Test content" }
            };
            var checkersServiceWithNullAutoCheck = new CheckersService(reviewServiceMock.Object, null);

            // Act
            var result = await checkersServiceWithNullAutoCheck.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }
    }
} 