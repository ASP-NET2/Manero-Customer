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
using Moq;
using Moq.Protected;
using Xunit;

namespace ManeroCustomerTest
{
    public class CategoryServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigurationSection;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            // Mock IConfigurationSection
            _mockConfigurationSection = new Mock<IConfigurationSection>();
            _mockConfigurationSection.Setup(x => x.Value).Returns("https://fakeurl.com/api/category");

            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x.GetSection("AzureFunctions:GetCategory")).Returns(_mockConfigurationSection.Object);

            // Mock HttpMessageHandler for HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Create instance of the service with mocked dependencies
            _categoryService = new CategoryService(_mockConfiguration.Object, _httpClient);
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnsCategoryList_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedCategories = new List<CategoryModel>
        {
            new CategoryModel { CategoryName = "Cat1", ImageLink = "link1" },
            new CategoryModel { CategoryName = "Cat2", ImageLink = "link2" }
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
                    Content = JsonContent.Create(expectedCategories)
                });

            // Act
            var result = await _categoryService.GetCategoryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.Equal(expectedCategories[0].CategoryName, result[0].CategoryName);
            Assert.Equal(expectedCategories[0].ImageLink, result[0].ImageLink);
            Assert.Equal(expectedCategories[1].CategoryName, result[1].CategoryName);
            Assert.Equal(expectedCategories[1].ImageLink, result[1].ImageLink);
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnsEmptyList_WhenApiCallFails()
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
            var result = await _categoryService.GetCategoryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SortCategoryAsync_ReturnsSortedCategories()
        {
            // Arrange
            var allCategories = new List<CategoryModel>
        {
            new CategoryModel { CategoryName = "Cat1", ImageLink = "link1" },
            new CategoryModel { CategoryName = "Cat2", ImageLink = "link2" },
            new CategoryModel { CategoryName = "Cat3", ImageLink = "link3" }
        };
            var unsorted = new List<string> { "Cat3", "Cat1" };
            var expected = new List<CategoryModel>
        {
            new CategoryModel { CategoryName = "Cat1", ImageLink = "link1" },
            new CategoryModel { CategoryName = "Cat3", ImageLink = "link3" }
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
                    Content = JsonContent.Create(allCategories)
                });

            // Act
            var result = await _categoryService.SortCategoryAsync(unsorted);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            Assert.Equal(expected[0].CategoryName, result[0].CategoryName);
            Assert.Equal(expected[0].ImageLink, result[0].ImageLink);
            Assert.Equal(expected[1].CategoryName, result[1].CategoryName);
            Assert.Equal(expected[1].ImageLink, result[1].ImageLink);
        }

        [Fact]
        public async Task SortCategoryAsync_ReturnsEmptyList_WhenApiCallFails()
        {
            // Arrange
            var unsorted = new List<string> { "Cat3", "Cat1" };

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
            var result = await _categoryService.SortCategoryAsync(unsorted);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

}
