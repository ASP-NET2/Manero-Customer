using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Manero_Customer.Data.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Manero_Customer.Services;
using Xunit;

namespace ManeroCustomerTest
{
    public class CartServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly cartService _cartService;

        public CartServiceTests()
        {
            // Mock HttpMessageHandler for HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Create instance of the service with mocked dependencies
            _cartService = new cartService(_httpClient);
        }

        [Fact]
        public async Task GetCartList_ReturnsCart_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedCart = new Cart { /* Initialize properties */ };
            var url = "https://maneroproductsfunction.azurewebsites.net/api/GetCustomerCart/32d541ff-63ea-4a4d-bc69-77e3c0d9b9f1?code=m7ibFqN_Rsi3NTYCkJetWZ90JJYiDk7lLkeHufoSZH3CAzFugGfWTQ%3D%3D";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri(url)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedCart)
                });

            // Act
            var result = await _cartService.GetCartList();

            // Assert
            Assert.NotNull(result);
 
        }

        [Fact]
        public async Task GetCartList_ReturnsNull_WhenApiCallFails()
        {
            // Arrange
            var url = "https://maneroproductsfunction.azurewebsites.net/api/GetCustomerCart/32d541ff-63ea-4a4d-bc69-77e3c0d9b9f1?code=m7ibFqN_Rsi3NTYCkJetWZ90JJYiDk7lLkeHufoSZH3CAzFugGfWTQ%3D%3D";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri(url)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _cartService.GetCartList();

            // Assert
            Assert.Null(result);
        }
    }

    public class cartService
    {
        private readonly HttpClient _httpClient;

        public cartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Cart?> GetCartList()
        {
            try
            {
                var test3 = "32d541ff-63ea-4a4d-bc69-77e3c0d9b9f1";
                var url = $"https://maneroproductsfunction.azurewebsites.net/api/GetCustomerCart/{test3}?code=m7ibFqN_Rsi3NTYCkJetWZ90JJYiDk7lLkeHufoSZH3CAzFugGfWTQ%3D%3D";
                var result = await _httpClient.GetFromJsonAsync<Cart>(url);
                return result!;
            }
            catch (Exception)
            {
                // Log the exception if necessary
            }
            return null!;
        }
    }
}
