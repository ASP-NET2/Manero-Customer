using Manero_Customer.Data;
using Manero_Customer.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Manero_Customer.Services;

public class AddressService
{
    private readonly HttpClient _http;
    private readonly ILogger<AddressService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;


    public AddressService(HttpClient http, ILogger<AddressService> logger, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _http = http;
        _logger = logger;
        _userManager = userManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AddressModel> GetAddressByIdAsync(int addressId)
    {
        _logger.LogInformation($"Getting address with Id: {addressId}");
        var response = await _http.GetFromJsonAsync<AddressModel>($"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}?code=t5rPM61neLc2cp-zLwAQcr9xaU529vItod0RCNklg0ftAzFu2m73MQ%3D%3D");

        if (response == null)
        {
            _logger.LogWarning($"No address was found with Id {addressId}");
        }

        return response ?? new AddressModel();
    }

    public async Task<List<AddressModel>> GetAddressesByUserAsync(int accountId)
    {
        var url = $"https://addressprovidermanero.azurewebsites.net/api/user/{accountId}/addresses?code=k7zE91OGmhvQCAauCE8qdYO_mQQGa_ZL02NHP4QHQCQ0AzFuPveOOA%3D%3D";
        _logger.LogInformation($"Fetching addresses for user ID: {accountId} from URL: {url}");

        try
        {
            var response = await _http.GetFromJsonAsync<List<AddressModel>>(url);
            _logger.LogInformation("Successfully fetched addresses from API.");
            return response ?? new List<AddressModel>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching addresses from API.");
            throw;
        }
    }

    public async Task AddAddressAsync(AddressModel model)
    {
        var url = $"https://addressprovidermanero.azurewebsites.net/api/addresses?code=U2iO9Q7CZvQkzEtM5aJhfwAaHPvkKnFJoTL7BUlu6sdZAzFu614sJw%3D%3D";
        // _logger.LogInformation($"Adding address for user ID: {AccountId} from URL: {url}");

        try
        {
            var addressModel = new AddressModel
            {
                AccountId = model.AccountId,
                AddressLine_1 = model.AddressLine_1,
                PostalCode = model.PostalCode,
                City = model.City,
            };
            var response = await _http.PostAsJsonAsync(url, model);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error adding address to API.");
            throw;
        }
    }

    public async Task UpdateAddressAsync(int addressId, UpdateAddressModel model)
    {
        var url = $"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}?code=-wt53LQPBqY7fJFebnkVnsUAw3sio_lTPzWOxu1yKyJEAzFu1vqQ1A%3D%3D";
        _logger.LogInformation($"Updating address ID: {addressId} from URL: {url}");

        try
        {
            var response = await _http.PutAsJsonAsync(url, model);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error updating address in API.");
            throw;
        }
    }

    public async Task DeleteAddressAsync(int addressId)
    {
        var url = $"https://addressprovidermanero.azurewebsites.net/api/addresses/{addressId}?code=2uUaBW9XAWs4FoA-J_pvePJgL3xrcU95Sq1SAmm5W94nAzFuqK3Wlg%3D%3D";
        _logger.LogInformation($"Deleting address ID: {addressId} from URL: {url}");

        try
        {
            var response = await _http.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error deleting address from API.");
            throw;
        }
    }

    public async Task<List<AddressModel>> GetAuthenticatedUserAddressesAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;

        if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
        {
            _logger.LogError("No user is authenticated");
            return new List<AddressModel>();
        }

        var identityUser = await _userManager.GetUserAsync(claimsPrincipal);
        if (identityUser != null)
        {
            if (int.TryParse(identityUser.Id, out int userId))
            {
                _logger.LogInformation($"{userId}");
                return await GetAddressesByUserAsync(userId);
            }
            else
            {
                _logger.LogError($"Failed to convert userId '{identityUser.Id}' to an integer.");
                return new List<AddressModel>();
            }
        }

        return new List<AddressModel>();
    }
}
