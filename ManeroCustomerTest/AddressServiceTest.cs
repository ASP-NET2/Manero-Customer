using Manero_Customer.Data.Models;
using Manero_Customer.Data;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Xunit;
using System.Net.Http.Json;
using Moq.Protected;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;



namespace ManeroCustomerTest;

public class AddressServiceTest
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<AddressService>> _mockLogger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    private readonly AddressService _addressService;

    public AddressServiceTest()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockLogger = new Mock<ILogger<AddressService>>();
        _userManager = MockUserManager<ApplicationUser>();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        _addressService = new AddressService(
            _httpClient,
            _mockLogger.Object,
            _userManager,
            _mockAuthStateProvider.Object);
    }

    private static UserManager<TUser> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<TUser>>();
        var userValidators = new List<IUserValidator<TUser>> { new Mock<IUserValidator<TUser>>().Object };
        var passwordValidators = new List<IPasswordValidator<TUser>> { new Mock<IPasswordValidator<TUser>>().Object };
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<TUser>>>();

        return new UserManager<TUser>(store.Object, options.Object, passwordHasher.Object, userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);
    }

    [Fact]
    public async Task GetAddressesByUserAsync_ReturnsAddresses_WhenAddressesExist()
    {
        // Arrange
        var accountId = 1;
        var expectedAddresses = new List<AddressModel> { new AddressModel { AccountId = accountId } };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedAddresses)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _addressService.GetAddressesByUserAsync(accountId);

        // Assert
        AssertEqualAddressModelLists(expectedAddresses, result);
        VerifyLogger(LogLevel.Information, $"Fetching addresses for user ID: {accountId} from URL: https://addressprovidermanero.azurewebsites.net/api/user/{accountId}/addresses?code=k7zE91OGmhvQCAauCE8qdYO_mQQGa_ZL02NHP4QHQCQ0AzFuPveOOA%3D%3D");
    }

    [Fact]
    public async Task GetAddressByIdAsync_ReturnsAddress_WhenAddressExists()
    {
        // Arrange
        var addressId = 1;
        var expectedAddress = new AddressModel { AddressId = addressId };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(expectedAddress)
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        var result = await _addressService.GetAddressByIdAsync(addressId);

        // Assert
        AssertEqualAddressModels(expectedAddress, result);
    }

    private void AssertEqualAddressModels(AddressModel expected, AddressModel actual)
    {
        Assert.Equal(expected.AddressId, actual.AddressId);
        Assert.Equal(expected.AccountId, actual.AccountId);
        Assert.Equal(expected.AddressLine_1, actual.AddressLine_1);
        Assert.Equal(expected.AddressTitle, actual.AddressTitle);
        Assert.Equal(expected.City, actual.City);
        Assert.Equal(expected.PostalCode, actual.PostalCode);
    }

    

    private void AssertEqualAddressModelLists(List<AddressModel> expected, List<AddressModel> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            AssertEqualAddressModels(expected[i], actual[i]);
        }
    }

    private void VerifyLogger(LogLevel logLevel, string message)
    {
        _mockLogger.Verify(
            x => x.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == message),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
    }
    [Fact]
    public async Task AddAddressAsync_CallsHttpClientPost_WhenCalled()
    {
        // Arrange
        var model = new AddressModel { AccountId = 1 };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString().Contains("https://addressprovidermanero.azurewebsites.net/api/addresses")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        await _addressService.AddAddressAsync(model);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri.ToString().Contains("https://addressprovidermanero.azurewebsites.net/api/addresses")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAddressAsync_CallsHttpClientPut_WhenCalled()
    {
        // Arrange
        var addressId = 1;
        var model = new UpdateAddressModel { AddressLine_1 = "New Address" };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri.ToString().Contains($"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        await _addressService.UpdateAddressAsync(addressId, model);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Put &&
                req.RequestUri.ToString().Contains($"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}")),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAddressAsync_CallsHttpClientDelete_WhenCalled()
    {
        // Arrange
        var addressId = 1;
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri.ToString().Contains($"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        // Act
        await _addressService.DeleteAddressAsync(addressId);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Delete &&
                req.RequestUri.ToString().Contains($"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}")),
            ItExpr.IsAny<CancellationToken>());
    }
}
