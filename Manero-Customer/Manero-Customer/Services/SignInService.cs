using Manero_Customer.Data;
using Manero_Customer.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Manero_Customer.Services;

public class SignInService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, NavigationManager navigationManager, ILogger<SignInService> logger, HttpClient httpClient)
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly ILogger<SignInService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> SignInUserAsync(SignInModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return "User does not exist";
        }

        if (!user.EmailConfirmed)
        {
            return "Email address is not confirmed.";
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result == null)
        {
            Console.WriteLine("SignInResult is null");
        }

        if (result!.Succeeded)
        {
            _navigationManager.NavigateTo("/");
            return "Success";
        }

        if (result.IsLockedOut)
        {
            _navigationManager.NavigateTo("/");
            return "User account locked out.";
        }

        return "Incorrect email or password.";
    }

    public async Task RegisterUser(SignUpModel form, string returnUrl)
    {
        var user = new ApplicationUser { UserName = form.Email, Email = form.Email };
        var result = await _userManager.CreateAsync(user, form.Password);

        if (result.Succeeded)
        {
            var accountUser = new AccountUserModel
            {
                IdentityUserId = user.Id,
                FirstName = form.FirstName,
                LastName = form.LastName
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:7096/api/CreateUser", accountUser);

            if (response.IsSuccessStatusCode)
            {
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    _navigationManager.NavigateTo($"/Account/VerifyAccount?email={user.Email}");
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _navigationManager.NavigateTo(returnUrl ?? "/");
                }
            }
            else
            {
                _logger.LogError("Error creating account user.");
            }
        }
        else
        {
            _logger.LogError("Error creating identity user.");
        }

    }

    public async Task SignOutAsync()
    {
        try
        {
            _logger.LogInformation("Starting to signout");
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User signout successfully");

            _navigationManager.NavigateTo("/Account/SignIn", true);
        }catch(Exception ex) 
        {
            _logger.LogError(ex, "An error has occured while signing out");
        }
    }

}
