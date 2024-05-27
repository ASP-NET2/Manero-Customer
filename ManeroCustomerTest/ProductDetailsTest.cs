using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace ManeroCustomerTest
{
    public class ProductDetailsServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<ILogger<ProductDetailsService>> _mockLogger;
        private readonly HttpClient _httpClient;
        private readonly ProductDetailsService _productDetailsService;

        public ProductDetailsServiceTests()
        {
            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();

            // Mock HttpMessageHandler for HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Mock ILogger
            _mockLogger = new Mock<ILogger<ProductDetailsService>>();

            // Create instance of the service with mocked dependencies
            _productDetailsService = new ProductDetailsService(_httpClient, _mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task MatchProductsByTitleAsync_ReturnsProductList_WhenApiCallIsSuccessful()
        {
            // Arrange
            var title = "TestTitle";
            var expectedProducts = new List<ProductDetailsModel>
            {
                new ProductDetailsModel { Title = "Product1", Author = "Author1", Price = 10, ImageUrl = "http://example.com/image1.jpg", ShortDescription = "Short Description 1", LongDescription = "Long Description 1", FormatName = "Format1" },
                new ProductDetailsModel { Title = "Product2", Author = "Author2", Price = 20, ImageUrl = "http://example.com/image2.jpg", ShortDescription = "Short Description 2", LongDescription = "Long Description 2", FormatName = "Format2" }
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains($"title={Uri.EscapeDataString(title)}")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedProducts)
                });

            // Act
            var result = await _productDetailsService.MatchProductsByTitleAsync(title);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count);
            Assert.Equal(expectedProducts[0].Title, result[0].Title);
            Assert.Equal(expectedProducts[1].Title, result[1].Title);
        }

        [Fact]
        public async Task MatchProductsByTitleAsync_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange
            var title = "TestTitle";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _productDetailsService.MatchProductsByTitleAsync(title);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task MatchProductsByTitleAsync_ReturnsEmptyList_WhenUnauthorized()
        {
            // Arrange
            var title = "TestTitle";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act
            var result = await _productDetailsService.MatchProductsByTitleAsync(title);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
