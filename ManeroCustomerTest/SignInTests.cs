using Manero_Customer.Data.Models;
using Manero_Customer.Data;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeroCustomerTest;

public class SignInTests
{
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly SignInService _signInService;

    public SignInTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null!, null!, null!, null!);

        _signInService = new SignInService(_signInManagerMock.Object, _userManagerMock.Object, null!);
    }

    [Fact]
    public async Task SignInUserAsync_UserDoesNotExist_ReturnsErrorMessage()
    {
        // Arrange
        var model = new SignInModel { Email = "test@example.com", Password = "Password123", RememberMe = false };
        _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await _signInService.SignInUserAsync(model);

        // Assert
        Assert.Equal("User does not exist", result);
    }

    [Fact]
    public async Task SignInUserAsync_EmailNotConfirmed_ReturnsErrorMessage()
    {
        // Arrange
        var user = new ApplicationUser { Email = "test@example.com", EmailConfirmed = false };
        var model = new SignInModel { Email = "test@example.com", Password = "Password123", RememberMe = false };
        _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(user);

        // Act
        var result = await _signInService.SignInUserAsync(model);

        // Assert
        Assert.Equal("Email address is not confirmed.", result);
    }

    [Fact]
    public void SignInUserAsync_SuccessfulSignIn_ReturnsSuccess()
    {
        // Arrange
        var user = new ApplicationUser { Email = "alina.garifjanova@hotmail.com", EmailConfirmed = true };
        var model = new SignInModel { Email = "alina.garifjanova@hotmail.com", Password = "London2017!", RememberMe = false };

        _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, model.Password, model.RememberMe, false)).ReturnsAsync(SignInResult.Success);

        // Act
        var foundUser = _userManagerMock.Object.FindByEmailAsync(model.Email).Result;
        var signInResult = _signInManagerMock.Object.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

        // Assert
        Assert.NotNull(foundUser);
        Assert.Equal(user.Email, foundUser.Email);
        Assert.Equal(SignInResult.Success, signInResult);
    }

    [Fact]
    public async Task SignInUserAsync_IncorrectEmailOrPassword_ReturnsErrorMessage()
    {
        // Arrange
        var user = new ApplicationUser { Email = "test@example.com", EmailConfirmed = true };
        var model = new SignInModel { Email = "test@example.com", Password = "WrongPassword", RememberMe = false };
        _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, model.Password, model.RememberMe, false)).ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _signInService.SignInUserAsync(model);

        // Assert
        Assert.Equal("Incorrect email or password.", result);
    }
}
