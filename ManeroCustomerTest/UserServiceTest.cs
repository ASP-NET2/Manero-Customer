using Manero_Customer.Data.Models;
using Manero_Customer.Data;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using System.Net.Http.Json;
using System.Net;
using System.Security.Claims;

namespace ManeroCustomerTest;

public class UserServiceTest
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockLogger = new Mock<ILogger<UserService>>();
        _mockUserManager = MockUserManager();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        _userService = new UserService(
            _httpClient,
            _mockLogger.Object,
            _mockUserManager.Object,
            _mockAuthStateProvider.Object);
    }

    private static Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
        var userValidators = new List<IUserValidator<ApplicationUser>> { new Mock<IUserValidator<ApplicationUser>>().Object };
        var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { new Mock<IPasswordValidator<ApplicationUser>>().Object };
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

        return new Mock<UserManager<ApplicationUser>>(store.Object, options.Object, passwordHasher.Object, userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);
    }

    [Fact]
    public async Task GetUserProfileAsync_ReturnsProfile_WhenSuccess()
    {
        // Arrange
        var userId = "123";
        var expectedProfile = new ProfileModel { IdentityUserId = userId };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedProfile)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/GetUserFunction/{userId}?code=Ry_Tr3kWZxm8TgDJbIPhKbHJyo1HqTVLz6dBsbDP3W-KAzFuiFNPFw%3D%3D")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _userService.GetUserProfileAsync(userId);

        // Assert
        AssertEqualProfileModels(expectedProfile, result);
    }

    [Fact]
    public async Task GetUserProfileAsync_ThrowsException_WhenHttpRequestException()
    {
        // Arrange
        var userId = "123";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/GetUserFunction/{userId}?code=Ry_Tr3kWZxm8TgDJbIPhKbHJyo1HqTVLz6dBsbDP3W-KAzFuiFNPFw%3D%3D")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _userService.GetUserProfileAsync(userId));
    }

    [Fact]
    public async Task UpdateUserProfileAsync_CallsApi_WhenSuccess()
    {
        // Arrange
        var userId = "123";
        var profileModel = new ProfileModel { IdentityUserId = userId };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/UpdateUserFunction/{userId}?code=kFu-wQD2Y8Blt7wbTJ_xwNcA3XQUMM55urhn6r-hbrkuAzFuOInniQ%3D%3D")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        await _userService.UpdateUserProfileAsync(userId, profileModel);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/UpdateUserFunction/{userId}?code=kFu-wQD2Y8Blt7wbTJ_xwNcA3XQUMM55urhn6r-hbrkuAzFuOInniQ%3D%3D")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetAuthenticatedUserProfileAsync_ReturnsProfile_WhenUserIsAuthenticated()
    {
        // Arrange
        var userId = "123";
        var expectedProfile = new ProfileModel { IdentityUserId = userId };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        var authState = Task.FromResult(new AuthenticationState(claimsPrincipal));

        _mockAuthStateProvider.Setup(m => m.GetAuthenticationStateAsync()).Returns(authState);

        var user = new ApplicationUser { Id = userId };
        _mockUserManager.Setup(um => um.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedProfile)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/GetUserFunction/{userId}?code=Ry_Tr3kWZxm8TgDJbIPhKbHJyo1HqTVLz6dBsbDP3W-KAzFuiFNPFw%3D%3D")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _userService.GetAuthenticatedUserProfileAsync();

        // Assert
        AssertEqualProfileModels(expectedProfile, result);
    }

    [Fact]
    public async Task DeleteUserAsync_ReturnsTrue_WhenSuccess()
    {
        // Arrange
        var userId = "123";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri == new Uri($"https://manerouserprovider.azurewebsites.net/api/DeleteUserFunction/{userId}?code=9N9Mbe3CYA7NGwd6s29TPw6-7iRcA_IHBHcVLfN2sIUVAzFuy9PSXw%3D%3D")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var user = new ApplicationUser { Id = userId };
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
    }

    private void AssertEqualProfileModels(ProfileModel expected, ProfileModel actual)
    {
        Assert.Equal(expected.IdentityUserId, actual.IdentityUserId);
        Assert.Equal(expected.AccountId, actual.AccountId);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.ImageUrl, actual.ImageUrl);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);

    }
}

