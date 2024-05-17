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
    public class SubCategoryServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigurationSection;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<ILogger<SubCategoryService>> _mockLogger;
        private readonly HttpClient _httpClient;
        private readonly SubCategoryService _subCategoryService;

        public SubCategoryServiceTests()
        {
            // Mock IConfigurationSection
            _mockConfigurationSection = new Mock<IConfigurationSection>();
            _mockConfigurationSection.Setup(x => x.Value).Returns("https://fakeurl.com/api/subcategory");

            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x.GetSection("AzureFunctions:GetSubCategory")).Returns(_mockConfigurationSection.Object);

            // Mock HttpMessageHandler for HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Mock ILogger
            _mockLogger = new Mock<ILogger<SubCategoryService>>();

            // Create instance of the service with mocked dependencies
            _subCategoryService = new SubCategoryService(_mockConfiguration.Object, _httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task GetSubCategoryAsync_ReturnsSubCategoryList_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedSubCategories = new List<SubCategoryModel>
            {
                new SubCategoryModel { SubCategoryName = "SubCat1", ImageLink = "link1" },
                new SubCategoryModel { SubCategoryName = "SubCat2", ImageLink = "link2" }
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedSubCategories)
                });

            // Act
            var result = await _subCategoryService.GetSubCategoryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSubCategories.Count, result.Count);
            Assert.Equal(expectedSubCategories[0].SubCategoryName, result[0].SubCategoryName);
            Assert.Equal(expectedSubCategories[0].ImageLink, result[0].ImageLink);
            Assert.Equal(expectedSubCategories[1].SubCategoryName, result[1].SubCategoryName);
            Assert.Equal(expectedSubCategories[1].ImageLink, result[1].ImageLink);
        }

        [Fact]
        public async Task GetSubCategoryAsync_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange
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
            var result = await _subCategoryService.GetSubCategoryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SortSubCategoryAsync_ReturnsSortedSubCategories()
        {
            // Arrange
            var allSubCategories = new List<SubCategoryModel>
            {
                new SubCategoryModel { SubCategoryName = "SubCat1", ImageLink = "link1" },
                new SubCategoryModel { SubCategoryName = "SubCat2", ImageLink = "link2" },
                new SubCategoryModel { SubCategoryName = "SubCat3", ImageLink = "link3" }
            };
            var unsorted = new List<string> { "SubCat3", "SubCat1" };
            var expected = new List<SubCategoryModel>
            {
                new SubCategoryModel { SubCategoryName = "SubCat1", ImageLink = "link1" },
                new SubCategoryModel { SubCategoryName = "SubCat3", ImageLink = "link3" }
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(allSubCategories)
                });

            // Act
            var result = await _subCategoryService.SortSubCategoryAsync(unsorted);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected[0].SubCategoryName, result[0].SubCategoryName);
            Assert.Equal(expected[0].ImageLink, result[0].ImageLink);
            Assert.Equal(expected[1].SubCategoryName, result[1].SubCategoryName);
            Assert.Equal(expected[1].ImageLink, result[1].ImageLink);
        }

        [Fact]
        public async Task SortSubCategoryAsync_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange
            var unsorted = new List<string> { "SubCat3", "SubCat1" };

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
            var result = await _subCategoryService.SortSubCategoryAsync(unsorted);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
