using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.ServiceProxy;
using Moq;
using Moq.Protected;
using WinUiApp.Data.Data;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyRunAutoCheckTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly HttpClient httpClient;
        private readonly OffensiveWordsServiceProxy offensiveWordsServiceProxy;
        private readonly string baseUrl = "https://test-api.com";

        public OffensiveWordsServiceProxyRunAutoCheckTest()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.offensiveWordsServiceProxy = new OffensiveWordsServiceProxy(this.baseUrl);

            // Use reflection to set the private httpClient field
            var httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(this.offensiveWordsServiceProxy, this.httpClient);
        }

        [Fact]
        public async Task RunAutoCheck_ValidReviews_ReturnsListOfOffensiveWords()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a good beer", DateTime.Now),
                new Review(2, Guid.NewGuid(), 2, 2.0f, "This beer contains offensive content", DateTime.Now)
            };

            var expectedOffensiveWords = new List<string> { "offensive", "content" };
            var jsonResponse = JsonSerializer.Serialize(expectedOffensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOffensiveWords.Count, result.Count);
            Assert.Equal(expectedOffensiveWords, result);
        }

        [Fact]
        public async Task RunAutoCheck_EmptyReviewsList_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>();
            var expectedOffensiveWords = new List<string>();
            var jsonResponse = JsonSerializer.Serialize(expectedOffensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_HttpRequestFails_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a good beer", DateTime.Now)
            };

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_HttpRequestThrowsException_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a good beer", DateTime.Now)
            };

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_ResponseContentIsNull_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a good beer", DateTime.Now)
            };

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null", Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_InvalidJsonResponse_ReturnsEmptyList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review(1, Guid.NewGuid(), 1, 4.5f, "This is a good beer", DateTime.Now)
            };

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/check"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("invalid json", Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.RunAutoCheck(reviews);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
} 