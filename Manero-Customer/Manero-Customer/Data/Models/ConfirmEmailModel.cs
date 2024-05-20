namespace Manero_Customer.Data.Models;

public class ConfirmEmailModel
{
    public string Email { get; set; } = null!;
    public string[] CodeDigits { get; set; } = new string[6];
}
