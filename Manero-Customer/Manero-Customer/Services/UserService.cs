﻿using Manero_Customer.Data.Models;
using Manero_Customer.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;

namespace Manero_Customer.Services;

public class UserService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserService(HttpClient http, ILogger<UserService> logger, UserManager<ApplicationUser> userManager, AuthenticationStateProvider authenticationStateProvider)
    {
        _http = http;
      //  _baseUrl = "http://localhost:7096";
        _logger = logger;
        _userManager = userManager;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ProfileModel> GetUserProfileAsync(string id)
    {
        var url = $"https://userprovider-manero.azurewebsites.net/api/GetUserFunction/{id}?code=6MvHEEIkcl3GlREG0UF6Ql0TvWYGjAwuxMNG6JG7v22aAzFue3OluA%3D%3D";
        _logger.LogInformation($"Fetching user profile from URL: {url}");

        try
        {
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var userProfile = await response.Content.ReadFromJsonAsync<ProfileModel>();
            if (userProfile == null)
            {
                _logger.LogWarning($"Profile data for user {id} not found.");
            }
            return userProfile!;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching user profile from API.");
            throw;
        }
    }

    public async Task UpdateUserProfileAsync(string id, ProfileModel model)
    {
        var url = $"https://userprovider-manero.azurewebsites.net/api/UpdateUserFunction/{id}?code=am_rEPAEf1CpsWB0gHRoLRWKQ3rvhvwCU1EYELc3n_c7AzFu5QUdGA%3D%3D";
        _logger.LogInformation($"Updating user profile from URL: {url}");

        try
        {
            var response = await _http.PostAsJsonAsync(url, model);
            response.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error updating user profile from API.");
            throw;
        }
    }

    public async Task<ProfileModel?> GetAuthenticatedUserProfileAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;

        if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
        {
            _logger.LogError("No user is authenticated");
            return null;
        }

        var identityUser = await _userManager.GetUserAsync(claimsPrincipal);
        if (identityUser != null)
        {

            var userId = identityUser.Id;
            _logger.LogInformation($"Fetching profile for userId: {userId}");

            return await GetUserProfileAsync(userId);
        }

        return null;
    }

    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        _logger.LogInformation($"Updating user in Identity system for userId: {user.Id}");
        try
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error updating user in identity system");
            return false;
        }
    }

    public async Task SaveUserProfileChangesAsync(ProfileModel userForm)
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var claimsPrincipal = authState.User;

        if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
        {
            _logger.LogError("No user is authenticated");
            return;
        }

        var identityUser = await _userManager.GetUserAsync(claimsPrincipal);
        if (identityUser != null)
        {
            identityUser.PhoneNumber = userForm.PhoneNumber;

            var result = await _userManager.UpdateAsync(identityUser);
            if (result.Succeeded)
            {
                var userId = identityUser.Id;
                _logger.LogInformation($"Updating profile for userId: {userId}");

                try
                {
                    await UpdateUserProfileAsync(userId, userForm);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error updating user profile in API.");
                }
            }
            else
            {
                _logger.LogError("Failed to update user in Identity system.");
            }
        }
    }


    public async Task<bool> DeleteUserAsync(string id)
    {
        _logger.LogInformation($"Deleting user account for userId: {id}");

        try
        {
            var profileUrl = $"https://userprovider-manero.azurewebsites.net/api/DeleteUserFunction/{id}?code=QRfVThVl8tAeFNEPL9OfUHA28SbUOdsUNBaqlMe4yBSVAzFu86zlOQ%3D%3D";
            var profileResponse = await _http.DeleteAsync(profileUrl);
            if (!profileResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to delete user profile from Azure Functions.");
                return false;
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    _logger.LogError("Failed to delete user from Identity system.");
                    return false;
                }
            }
            else
            {
                _logger.LogWarning("User not found in Identity system.");
            }

            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error deleting user account.");
            return false;
        }
    }
}