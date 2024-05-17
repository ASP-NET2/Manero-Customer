namespace ManeroCustomerTest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Moq.Protected;
    using Xunit;

    public class ProductCategoryModel
    {
        public string Title { get; set; }  // Uppdaterad egenskap
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class MyService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MyService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<List<ProductCategoryModel>> GetProducts()
        {
            try
            {
                var url = _configuration["AzureFunctions:GetAllProducts"];
                var result = await _httpClient.GetFromJsonAsync<List<ProductCategoryModel>>(url);
                return result ?? new List<ProductCategoryModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ProductCategoryModel>();
            }
        }
    }

    public class MyServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IConfigurationSection> _mockConfigurationSection;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly MyService _myService;

        public MyServiceTests()
        {
            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfigurationSection = new Mock<IConfigurationSection>();

            // Ställ in den falska URL:en
            _mockConfigurationSection.Setup(a => a.Value).Returns("https://fakeurl.com/api/products");

            // Konfigurera mock för att returnera sektionen
            _mockConfiguration.Setup(a => a.GetSection("AzureFunctions:GetAllProducts")).Returns(_mockConfigurationSection.Object);

            // Mock HttpMessageHandler för HttpClient
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            // Skapa instans av tjänsten med mockade beroenden
            _myService = new MyService(_mockConfiguration.Object, _httpClient);
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
            var result = await _myService.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

}