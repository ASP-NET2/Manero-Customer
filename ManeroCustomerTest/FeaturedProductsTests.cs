using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Manero_Customer.Services;
using Manero_Customer.Data.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ManeroCustomerTest
{
    public class FeaturedProductsTests
    {
        [Fact]
        public async Task FilterProduct_ReturnsFeaturedProducts()
        {
            // Arrange
            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(x => x.Value).Returns("https://fakeapi.com");

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("AzureFunctions:GetAllProducts")).Returns(mockConfigSection.Object);

            var mockLogger = new Mock<ILogger<ProductService>>();

            var featuredProducts = new List<ProductModel>
            {
                new ProductModel { Id = "1", Title = "Product 1", FeaturedProduct = true },
                new ProductModel { Id = "2", Title = "Product 2", FeaturedProduct = false },
                new ProductModel { Id = "3", Title = "Product 3", FeaturedProduct = true }
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("featuredProduct=true")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(featuredProducts.Where(p => p.FeaturedProduct).ToList())
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var productService = new ProductService(httpClient, mockConfig.Object, mockLogger.Object);

            // Act
            var filters = new Dictionary<string, string>
            {
                { "featuredProduct", "true" }
            };
            var products = await productService.FilterProduct(filters);

            // Assert
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
            Assert.All(products, p => Assert.True(p.FeaturedProduct));
        }
    }
}
