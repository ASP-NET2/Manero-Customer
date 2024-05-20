using Azure.Messaging.ServiceBus;
using Manero_Customer.Data.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace Manero_Customer.Services;

public class VerifyAccountService
{
    private readonly ServiceBusSender _sender;
    private readonly NavigationManager? _navigationManager;

    public VerifyAccountService(ServiceBusSender sender, NavigationManager? navigationManager = null)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _navigationManager = navigationManager;
    }

    public async Task SendVerificationCodeAsync(string email)
    {
        try
        {
            var verificationModel = new VerificationRequest { Email = email };
            var jsonMessage = JsonConvert.SerializeObject(verificationModel);
            var servicebusMessage = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(servicebusMessage);

            _navigationManager?.NavigateTo($"/Account/ConfirmAccount?email={email}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending verification code: {ex.Message}");
            throw;
        }
    }
}
