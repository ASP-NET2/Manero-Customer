using Manero_Customer.Data;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components;
using Moq.Protected;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ManeroCustomerTest;

public class ConfirmAccountServiceTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private ApplicationDbContext _dbContext;
    private Mock<NavigationManager> _navigationManagerMock;
    private ConfirmAccountService _confirmAccountService;

    public ConfirmAccountServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null!, null!, null!, null!, null!, null!, null!, null!);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _navigationManagerMock = new Mock<NavigationManager>();

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://manero-verificationprovider.azurewebsites.net")
        };

        _confirmAccountService = new ConfirmAccountService(httpClient, _userManagerMock.Object, _dbContext, _navigationManagerMock.Object);
    }
    

    [Fact]
    public async Task VerifyCodeAsync_Unauthorized_ReturnsUnauthorized()
    {
        // Arrange
        var email = "test@example.com";
        var codeDigits = new[] { "1", "2", "3", "4", "5", "6" };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _confirmAccountService.VerifyCodeAsync(email, codeDigits);

        // Assert
        Assert.Equal("Un authorized", result);
    }

    [Fact]
    public async Task VerifyCodeAsync_NotFound_ReturnsNotFound()
    {
        // Arrange
        var email = "test@example.com";
        var codeDigits = new[] { "1", "2", "3", "4", "5", "6" };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _confirmAccountService.VerifyCodeAsync(email, codeDigits);

        // Assert
        Assert.Equal("Not Found", result);
    }

    [Fact]
    public async Task VerifyCodeAsync_BadRequest_ReturnsBadRequest()
    {
        // Arrange
        var email = "test@example.com";
        var codeDigits = new[] { "1", "2", "3", "4", "5", "6" };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _confirmAccountService.VerifyCodeAsync(email, codeDigits);

        // Assert
        Assert.Equal("Bad request", result);
    }

    [Fact]
    public async Task VerifyCodeAsync_OtherStatusCode_ReturnsSomethingWentWrong()
    {
        // Arrange
        var email = "test@example.com";
        var codeDigits = new[] { "1", "2", "3", "4", "5", "6" };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _confirmAccountService.VerifyCodeAsync(email, codeDigits);

        // Assert
        Assert.Equal("something went wrong", result);
    }
}