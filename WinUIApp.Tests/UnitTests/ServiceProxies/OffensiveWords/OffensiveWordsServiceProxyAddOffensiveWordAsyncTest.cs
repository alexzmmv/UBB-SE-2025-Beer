using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Model.AdminDashboard;
using DataAccess.ServiceProxy;
using Moq;
using Moq.Protected;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyAddOffensiveWordAsyncTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly HttpClient httpClient;
        private readonly OffensiveWordsServiceProxy offensiveWordsServiceProxy;
        private readonly string baseUrl = "https://test-api.com";

        public OffensiveWordsServiceProxyAddOffensiveWordAsyncTest()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.offensiveWordsServiceProxy = new OffensiveWordsServiceProxy(this.baseUrl);

            // Use reflection to set the private httpClient field
            System.Reflection.FieldInfo? httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(this.offensiveWordsServiceProxy, this.httpClient);
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ValidNewWord_CallsGetRequest()
        {
            // Arrange
            string newWord = "newbadword";
            List<OffensiveWord> existingWords = new()
            {
                new OffensiveWord("existingword1"),
                new OffensiveWord("existingword2")
            };

            string? getResponse = JsonSerializer.Serialize(existingWords);

            // Setup GET request to check existing words
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Setup POST request to add new word
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_ValidNewWord_CallsPostRequest()
        {
            // Arrange
            string newWord = "newbadword";
            List<OffensiveWord> existingWords = new()
            {
                new OffensiveWord("existingword1"),
                new OffensiveWord("existingword2")
            };

            string? getResponse = JsonSerializer.Serialize(existingWords);

            // Setup GET request to check existing words
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Setup POST request to add new word
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_WordAlreadyExists_CallsGetRequest()
        {
            // Arrange
            string existingWord = "existingword1";
            List<OffensiveWord> existingWords = new()
            {
                new OffensiveWord("existingword1"),
                new OffensiveWord("existingword2")
            };

            string? getResponse = JsonSerializer.Serialize(existingWords);

            // Setup GET request to check existing words
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(existingWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_WordAlreadyExists_DoesNotCallPostRequest()
        {
            // Arrange
            string existingWord = "existingword1";
            List<OffensiveWord> existingWords = new()
            {
                new OffensiveWord("existingword1"),
                new OffensiveWord("existingword2")
            };

            string? getResponse = JsonSerializer.Serialize(existingWords);

            // Setup GET request to check existing words
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(existingWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_NullWord_DoesNotCallAnyRequest()
        {
            // Arrange
            string nullWord = null;

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(nullWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_EmptyWord_DoesNotCallAnyRequest()
        {
            // Arrange
            string emptyWord = string.Empty;

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(emptyWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_WhitespaceWord_DoesNotCallAnyRequest()
        {
            // Arrange
            string whitespaceWord = "   ";

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(whitespaceWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_GetRequestFails_ThrowsException()
        {
            // Arrange
            string newWord = "newbadword";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord));
        }

        [Fact]
        public async Task AddOffensiveWordAsync_PostRequestFails_ThrowsException()
        {
            // Arrange
            string newWord = "newbadword";
            List<OffensiveWord> existingWords = new();
            string? getResponse = JsonSerializer.Serialize(existingWords);

            // Setup successful GET request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Setup failing POST request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord));
        }

        [Fact]
        public async Task AddOffensiveWordAsync_EmptyExistingWordsList_CallsGetRequest()
        {
            // Arrange
            string newWord = "newbadword";
            List<OffensiveWord> emptyWordsList = new();
            string? getResponse = JsonSerializer.Serialize(emptyWordsList);

            // Setup GET request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Setup POST request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddOffensiveWordAsync_EmptyExistingWordsList_CallsPostRequest()
        {
            // Arrange
            string newWord = "newbadword";
            List<OffensiveWord> emptyWordsList = new();
            string? getResponse = JsonSerializer.Serialize(emptyWordsList);

            // Setup GET request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(getResponse, Encoding.UTF8, "application/json")
                });

            // Setup POST request
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.AddOffensiveWordAsync(newWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords/add"),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}