using System.ComponentModel.DataAnnotations;

namespace Manero_Customer.Data.Models;

public class ProfileModel
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? Location { get; set; }
    public string? IdentityUserId { get; set; }
}
