using Manero_Customer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using Manero_Customer.Data.Models;

namespace Manero_Customer.Services;

public class ConfirmAccountService(HttpClient httpClient, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, NavigationManager navigation)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly NavigationManager _navigation = navigation;

    public async Task<string> VerifyCodeAsync(string email, string[] codeDigits)
    {
        var joinedCode = string.Join("", codeDigits);

        var message = new VerificationMessage
        {
            Email = email,
            VerificationCode = joinedCode
        };

        var jsonMessage = JsonConvert.SerializeObject(message);
        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://manero-verificationprovider.azurewebsites.net/api/Verification", content);

        if (response.IsSuccessStatusCode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && !user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _dbContext.SaveChangesAsync();
            }

            _navigation.NavigateTo("/Account/login");
            return "Success";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return "Un authorized";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return "Not Found";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return "Bad request";
        }
        else
        {
            return "something went wrong";
        }
    }

}
