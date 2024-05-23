using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Manero_Customer.Components.Pages;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace ManeroCustomerTest
{
    public class ProductServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigurationSection;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly HttpClient _httpClient;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            // Mock IConfigurationSection
            _mockConfigurationSection = new Mock<IConfigurationSection>();
            _mockConfigurationSection.Setup(x => x.Value).Returns("https://fakeurl.com/api/products");

            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(x => x.GetSection("AzureFunctions:GetAllProducts")).Returns(_mockConfigurationSection.Object);

            // Mock HttpMessageHandler for HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Mock ILogger
            _mockLogger = new Mock<ILogger<ProductService>>();

            // Create instance of the service with mocked dependencies
            _productService = new ProductService(_httpClient, _mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsProductList_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedProducts = new List<ProductCategoryModel>
            {
                new ProductCategoryModel { Title = "Product1", Category = "Category1", SubCategory = "SubCat1", Author = "Author1", Price = "10.00" },
                new ProductCategoryModel { Title = "Product2", Category = "Category2", SubCategory = "SubCat2", Author = "Author2", Price = "20.00" }
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
                    Content = JsonContent.Create(expectedProducts)
                });

            // Act
            var result = await _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count);
            Assert.Equal(expectedProducts[0].Title, result[0].Title);
            Assert.Equal(expectedProducts[1].Title, result[1].Title);
        }

        [Fact]
        public async Task GetProducts_ReturnsEmptyList_WhenApiCallFails()
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
            var result = await _productService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FilterProduct_ReturnsFilteredProducts()
        {
            // Arrange
            var allProducts = new List<ProductCategoryModel>
            {
                new ProductCategoryModel { Title = "Product1", Category = "Category1", SubCategory = "SubCat1", Author = "Author1", Price = "10.00" },
                new ProductCategoryModel { Title = "Product2", Category = "Category2", SubCategory = "SubCat2", Author = "Author2", Price = "20.00" },
                new ProductCategoryModel { Title = "Product3", Category = "Category1", SubCategory = "SubCat3", Author = "Author3", Price = "30.00" }
            };

            var filteredProducts = new List<ProductCategoryModel>
            {
                new ProductCategoryModel { Title = "Product1", Category = "Category1", SubCategory = "SubCat1", Author = "Author1", Price = "10.00" },
                new ProductCategoryModel { Title = "Product3", Category = "Category1", SubCategory = "SubCat3", Author = "Author3", Price = "30.00" }
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
                    Content = JsonContent.Create(filteredProducts)
                });

            var SubCategory = new Dictionary<string, string>
            {
                { "Subcategory",  "SubCategory" }
            };
           
            // Act

            var result = await _productService.FilterProduct(SubCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(filteredProducts.Count, result.Count);
            Assert.Equal(filteredProducts[0].Title, result[0].Title);
            Assert.Equal(filteredProducts[1].Title, result[1].Title);
        }

        [Fact]
        public async Task FilterProduct_ReturnsEmptyList_WhenApiCallFails()
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
            var SubCategory = new Dictionary<string, string>
            {
                { "Subcategory",  "SubCategory" }
            };

            // Act
            var result = await _productService.FilterProduct(SubCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
