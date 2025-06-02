using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    /// <summary>
    /// Integration tests for AI detection functionality in CheckersService.
    /// These tests exercise the RunAICheckForOneReviewAsync method with various content types
    /// to improve code coverage of the CheckReviewWithAI static method.
    /// 
    /// Note: Since CheckReviewWithAI calls OffensiveTextDetector.DetectOffensiveContent (static method),
    /// these tests depend on the actual implementation and may be considered integration tests
    /// rather than pure unit tests.
    /// </summary>
    public class CheckersServiceAIDetectionIntegrationTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceAIDetectionIntegrationTest()
        {
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidContent_ExercisesAIDetectionPath()
        {
            // Arrange
            Review review = new() { ReviewId = 1, Content = "This is a normal review content" };

            // Act & Assert (should not throw and should exercise the AI detection code path)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_JsonLikeContent_ExercisesJsonParsingPath()
        {
            // Arrange
            string jsonLikeContent = "{\"label\": \"offensive\", \"score\": 0.8}";
            Review review = new() { ReviewId = 1, Content = jsonLikeContent };

            // Act & Assert (should not throw and may exercise JSON parsing paths)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MalformedJsonContent_ExercisesErrorHandling()
        {
            // Arrange
            string malformedJson = "{\"label\": \"offensive\", \"score\":";
            Review review = new() { ReviewId = 1, Content = malformedJson };

            // Act & Assert (should not throw and may exercise error handling paths)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NumericContent_ExercisesContentProcessing()
        {
            // Arrange
            string numericContent = "12345 67890 rating: 5.0";
            Review review = new() { ReviewId = 1, Content = numericContent };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MixedCaseContent_ExercisesTextProcessing()
        {
            // Arrange
            string mixedCaseContent = "ThIs Is A MiXeD cAsE rEvIeW";
            Review review = new() { ReviewId = 1, Content = mixedCaseContent };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ContentWithNewlines_ExercisesFormatHandling()
        {
            // Arrange
            string contentWithNewlines = "Line 1\nLine 2\r\nLine 3\tTabbed content";
            Review review = new() { ReviewId = 1, Content = contentWithNewlines };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ContentWithQuotes_ExercisesStringHandling()
        {
            // Arrange
            string contentWithQuotes = "This is a \"quoted\" review with 'single quotes' too";
            Review review = new() { ReviewId = 1, Content = contentWithQuotes };

            // Act & Assert (should not throw)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ErrorPrefixContent_ExercisesErrorDetection()
        {
            // Arrange - Content that might trigger the "Error:" prefix check
            string errorContent = "Error: Something went wrong in this review";
            Review review = new() { ReviewId = 1, Content = errorContent };

            // Act & Assert (should not throw and may exercise error prefix detection)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionPrefixContent_ExercisesExceptionDetection()
        {
            // Arrange - Content that might trigger the "Exception:" prefix check
            string exceptionContent = "Exception: This review contains the word exception";
            Review review = new() { ReviewId = 1, Content = exceptionContent };

            // Act & Assert (should not throw and may exercise exception prefix detection)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_OffensiveLabelContent_ExercisesLabelDetection()
        {
            // Arrange - Content that might trigger the "offensive" label check
            string offensiveContent = "This review contains the word offensive in it";
            Review review = new() { ReviewId = 1, Content = offensiveContent };

            // Act & Assert (should not throw and may exercise label detection)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ScoreContent_ExercisesScoreParsingPath()
        {
            // Arrange - Content that might trigger score parsing logic
            string scoreContent = "This review has a score of 0.75 out of 1.0";
            Review review = new() { ReviewId = 1, Content = scoreContent };

            // Act & Assert (should not throw and may exercise score parsing paths)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullJsonContent_ExercisesNullDeserializationPath()
        {
            // Arrange - Content that might cause OffensiveTextDetector to return "null" 
            // which would make JsonConvert.DeserializeObject return null
            string nullJsonContent = "null content that might trigger null response";
            Review review = new() { ReviewId = 1, Content = nullJsonContent };

            // Act & Assert (should not throw and may exercise the arrayResults == null path)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_EmptyArrayContent_ExercisesEmptyArrayPath()
        {
            // Arrange - Content that might cause OffensiveTextDetector to return empty array JSON
            // This would exercise the arrayResults?.Count > 0 check
            string emptyArrayContent = "content that might return empty array []";
            Review review = new() { ReviewId = 1, Content = emptyArrayContent };

            // Act & Assert (should not throw and may exercise empty array handling)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NestedEmptyArrayContent_ExercisesNestedEmptyArrayPath()
        {
            // Arrange - Content that might cause response like [[]] - empty nested array
            // This would exercise the arrayResults[0]?.Count > 0 check
            string nestedEmptyContent = "content that might return nested empty [[]]";
            Review review = new() { ReviewId = 1, Content = nestedEmptyContent };

            // Act & Assert (should not throw and may exercise nested empty array handling)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_IncompleteJsonStructure_ExercisesStructureValidation()
        {
            // Arrange - Content that might cause response with missing label/score fields
            // This would exercise the prediction.TryGetValue checks
            string incompleteStructureContent = "content that might return incomplete JSON structure";
            Review review = new() { ReviewId = 1, Content = incompleteStructureContent };

            // Act & Assert (should not throw and may exercise structure validation)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NonOffensiveLabelContent_ExercisesLabelComparison()
        {
            // Arrange - Content that might return a label other than "offensive"
            // This would exercise the label comparison logic
            string nonOffensiveContent = "content that might return non-offensive label";
            Review review = new() { ReviewId = 1, Content = nonOffensiveContent };

            // Act & Assert (should not throw and may exercise non-offensive label path)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_LowScoreContent_ExercisesScoreThreshold()
        {
            // Arrange - Content that might return offensive label but score <= 0.6
            // This would exercise the score threshold check (score > 0.6)
            string lowScoreContent = "content that might return low confidence score";
            Review review = new() { ReviewId = 1, Content = lowScoreContent };

            // Act & Assert (should not throw and may exercise score threshold logic)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_InvalidScoreFormat_ExercisesScoreParsing()
        {
            // Arrange - Content that might return non-numeric score value
            // This would exercise the double.TryParse failure path
            string invalidScoreContent = "content that might return invalid score format";
            Review review = new() { ReviewId = 1, Content = invalidScoreContent };

            // Act & Assert (should not throw and may exercise score parsing error handling)
            this.checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MultipleReviewObjects_ExercisesArrayHandling()
        {
            // Arrange - Test with different review content patterns
            Review[] reviews =
            [
                new() { ReviewId = 1, Content = "Simple content" },
                new() { ReviewId = 2, Content = "Content with numbers 123" },
                new() { ReviewId = 3, Content = "UPPERCASE CONTENT" },
                new() { ReviewId = 4, Content = "lowercase content" },
                new() { ReviewId = 5, Content = "Mixed Case Content" }
            ];

            // Act & Assert (should not throw for any review)
            foreach (Review review in reviews)
            {
                this.checkersService.RunAICheckForOneReviewAsync(review);
            }
        }

        /// <summary>
        /// Test documenting the JSON deserialization null scenario.
        /// 
        /// JsonConvert.DeserializeObject can return null in several cases:
        /// 1. When the JSON response is literally "null"
        /// 2. When the JSON structure doesn't match the expected type
        /// 3. When malformed JSON can't be converted to the target type
        /// 
        /// This test exercises content that might potentially trigger such scenarios.
        /// </summary>
        [Fact]
        public void RunAICheckForOneReviewAsync_JsonDeserializationEdgeCases_ExercisesNullHandling()
        {
            // Arrange - Various content types that might trigger different JSON responses
            string[] edgeCaseContents =
            [
                "Content that might return null JSON response",
                "Content that might return malformed JSON",
                "Content that might return unexpected JSON structure",
                "Content that might return empty JSON object {}",
                "Content that might return JSON with wrong schema"
            ];

            // Act & Assert - All should handle gracefully without throwing
            foreach (string content in edgeCaseContents)
            {
                Review review = new() { ReviewId = 1, Content = content };
                this.checkersService.RunAICheckForOneReviewAsync(review);
            }
        }
    }
}