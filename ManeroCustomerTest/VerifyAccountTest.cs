using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Bunit;
using Manero_Customer.Data.Models;
using Manero_Customer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ManeroCustomerTest;
public class VerifyAccountTests
{
    private readonly Mock<ServiceBusSender> _serviceBusSenderMock;
    private readonly VerifyAccountService _verifyAccountService;

    public VerifyAccountTests()
    {
        _serviceBusSenderMock = new Mock<ServiceBusSender>();
        _verifyAccountService = new VerifyAccountService(_serviceBusSenderMock.Object, null);
    }


    [Fact]
    public async Task SendVerificationCodeAsync_ThrowsException()
    {
        // Arrange
        var email = "test@example.com";
        _serviceBusSenderMock
            .Setup(s => s.SendMessageAsync(It.IsAny<ServiceBusMessage>(), default))
            .ThrowsAsync(new Exception("ServiceBus error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _verifyAccountService.SendVerificationCodeAsync(email));
        Assert.Equal("ServiceBus error", exception.Message);
    }
}