using System.ComponentModel.DataAnnotations;

namespace Manero_Customer.Data.Models;

public class ProfileModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string IdentityUserId { get; set; }
}
