using Manero_Customer.Data;
using Manero_Customer.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Manero_Customer.Services;

public class SignInService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, NavigationManager navigationManager)
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly NavigationManager _navigationManager = navigationManager;


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

        if (result.Succeeded)
        {
            _navigationManager.NavigateTo("/");
            return "Success";
        }

        if (result.IsLockedOut)
        {
            _navigationManager.NavigateTo("Account/Lockout");
            return "User account locked out.";
        }

        return "Incorrect email or password.";
    }

}
