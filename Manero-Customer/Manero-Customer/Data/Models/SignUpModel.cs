using System.ComponentModel.DataAnnotations;

namespace Manero_Customer.Data.Models;

public class SignUpModel
{
    [Required]
    [Display(Name = "FIRST NAME")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "LAST NAME")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*\.[a-zA-Z]{2,}$", ErrorMessage = "An valid email address is required")]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_])(?!.*\s).{8,15}$", ErrorMessage = "A valid password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "PASSWORD")]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Display(Name = "CONFIRM PASSWORD")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = "";


    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions")]
    public bool TermsAndConditions { get; set; }
}
