using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.ServiceProxy;
using Moq;
using Moq.Protected;
using WinUiApp.Data.Data;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyRunAICheckForOneReviewAsyncTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly HttpClient httpClient;
        private readonly OffensiveWordsServiceProxy offensiveWordsServiceProxy;
        private readonly string baseUrl = "https://test-api.com";

        public OffensiveWordsServiceProxyRunAICheckForOneReviewAsyncTest()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.offensiveWordsServiceProxy = new OffensiveWordsServiceProxy(this.baseUrl);

            // Use reflection to set the private httpClient field
            var httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(this.offensiveWordsServiceProxy, this.httpClient);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidReview_CallsAPISuccessfully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a great beer!", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            // Since this is an async void method, we need to wait a bit to ensure the call is made
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithEmptyContent_CallsAPISuccessfully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullReview_DoesNotCallAPI()
        {
            // Arrange
            Review nullReview = null;

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(nullReview);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithNullContent_DoesNotCallAPI()
        {
            // Arrange
            var review = new Review();
            review.Content = null;

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_HTTPRequestFails_HandlesExceptionGracefully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a test review", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());

            // The method should handle the exception gracefully and not throw
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_HTTPRequestThrowsException_HandlesExceptionGracefully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a test review", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());

            // The method should handle the exception gracefully and not throw
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithLongContent_CallsAPISuccessfully()
        {
            // Arrange
            string longContent = new string('a', 10000); // Very long content
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, longContent, DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithSpecialCharacters_CallsAPISuccessfully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "Review with special chars: !@#$%^&*()", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_Timeout_HandlesExceptionGracefully()
        {
            // Arrange
            var review = new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a test review", DateTime.Now);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Request timeout"));

            // Act
            this.offensiveWordsServiceProxy.RunAICheckForOneReviewAsync(review);

            // Assert
            Thread.Sleep(100);

            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/checkOne"),
                ItExpr.IsAny<CancellationToken>());

            // The method should handle the timeout gracefully and not throw
        }
    }
} 