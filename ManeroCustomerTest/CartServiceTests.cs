using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

namespace ManeroCustomerTest;
public class CartServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _configurationMock = new Mock<IConfiguration>();
        _cartService = new CartService(_httpClient, _configurationMock.Object);
    }

    [Fact]
    public async Task GetCartList_ReturnsCart_WhenApiCallIsSuccessful()
    {
        // Arrange
        var cartId = "test-id";
        var cart = new Cart { Id = cartId };
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(cart)
            });

        // Act
        var result = await _cartService.GetCartList(cartId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartId, result.Id);
    }

    [Fact]
    public async Task GetCartList_ReturnsNull_WhenApiCallFails()
    {
        // Arrange
        var cartId = "test-id";
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act
        var result = await _cartService.GetCartList(cartId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetGuid_ReturnsValidGuid()
    {
        // Act
        var result = _cartService.GetGuid();

        // Assert
        Assert.True(Guid.TryParse(result, out _));
    }

    [Fact]
    public async Task CreateCustomerCart_ReturnsCart_WhenApiCallIsSuccessful()
    {
        // Arrange
        var cartId = "test-id";
        var cart = new Cart { Id = cartId };
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(cart)
            });

        // Act
        var result = await _cartService.CreateCustomerCart(cartId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartId, result.Id);
    }

    //[Fact]
    //public async Task AddToCart_ReturnsUpdatedCart_WhenApiCallIsSuccessful()
    //{
    //    // Uppdatera detta testet när jag har en SessionId 
    //    // Arrange
    //    var cartId = "test-id";
    //    var product = new ProductModel { Id = "prod-1", Title = "Product 1", Price = 10 };
    //    var cart = new Cart { Id = cartId };
    //    _httpMessageHandlerMock.Protected()
    //        .Setup<Task<HttpResponseMessage>>(
    //            "SendAsync",
    //            ItExpr.IsAny<HttpRequestMessage>(),
    //            ItExpr.IsAny<CancellationToken>())
    //        .ReturnsAsync(new HttpResponseMessage
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Content = JsonContent.Create(cart)
    //        });

    //    // Act
    //    var result = await _cartService.AddToCart(product);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(cartId, result.Id);
    //}

    //[Fact]
    //public async Task DecreaseProductQuantity_ReturnsUpdatedCart_WhenApiCallIsSuccessful()
    //{
    //    // Arrange
    //    var cartId = "test-id";
    //    var productId = "prod-1";
    //    var cart = new Cart { Id = cartId };
    //    _httpMessageHandlerMock.Protected()
    //        .Setup<Task<HttpResponseMessage>>(
    //            "SendAsync",
    //            ItExpr.IsAny<HttpRequestMessage>(),
    //            ItExpr.IsAny<CancellationToken>())
    //        .ReturnsAsync(new HttpResponseMessage
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Content = JsonContent.Create(cart)
    //        });

    //    // Act
    //    var result = await _cartService.DecreaseProductQuantity(cartId, productId);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(cartId, result.Id);
    //}

    [Fact]
    public async Task DeleteProductFromCart_ReturnsUpdatedCart_WhenApiCallIsSuccessful()
    {
        // Arrange
        var cartId = "test-id";
        var productId = "prod-1";
        var cart = new Cart { Id = cartId };
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(cart)
            });

        // Act
        var result = await _cartService.DeleteProductFromCart(cartId, productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartId, result.Id);
    }
}
