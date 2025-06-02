using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    /// <summary>
    /// Tests for RunAICheckForOneReviewAsync method.
    /// Note: The CheckReviewWithAI method calls a static OffensiveTextDetector.DetectOffensiveContent method,
    /// which makes it difficult to test the AI detection logic paths in unit tests.
    /// These tests focus on the controllable behavior around the static method call.
    /// </summary>
    public class CheckersServiceRunAICheckForOneReviewAsyncTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceRunAICheckForOneReviewAsyncTest()
        {
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullReview_DoesNotCallReviewService()
        {
            // Act
            this.checkersService.RunAICheckForOneReviewAsync(null);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithNullContent_DoesNotCallReviewService()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = null };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithNullContent_DoesNotCallResetReviewFlags()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = null };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.ResetReviewFlags(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidReviewNotOffensive_DoesNotCallHideReview()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "Good content" };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidReviewNotOffensive_DoesNotCallResetReviewFlags()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "Good content" };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.ResetReviewFlags(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_EmptyContent_DoesNotCallReviewService()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = string.Empty };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_WhitespaceContent_DoesNotCallReviewService()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "   " };

            // Act
            this.checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "Test content" };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionInReviewService_DoesNotThrow()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "Test content" };
            this.reviewServiceMock.Setup(x => x.HideReview(It.IsAny<int>())).Throws(new Exception("Service error"));

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullReviewService_DoesNotThrow()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "Test content" };
            CheckersService checkersServiceWithNullReviewService = new(null, this.autoCheckMock.Object);

            // Act & Assert (should not throw)
            checkersServiceWithNullReviewService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_LongContent_DoesNotThrow()
        {
            // Arrange
            string longContent = new string('a', 10000); // Very long content
            Review review = new() { ReviewId = 1, Content = longContent };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_SpecialCharactersContent_DoesNotThrow()
        {
            // Arrange
            string specialContent = "!@#$%^&*()_+{}|:\"<>?[]\\;',./ test content";
            Review review = new() { ReviewId = 1, Content = specialContent };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_UnicodeContent_DoesNotThrow()
        {
            // Arrange
            string unicodeContent = "Test content with unicode: 🚀 こんにちは العربية";
            Review review = new() { ReviewId = 1, Content = unicodeContent };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }
    }
}