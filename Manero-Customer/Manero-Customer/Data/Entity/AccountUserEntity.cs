using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manero_Customer.Data.Entity;

public class AccountUserEntity 
{
    [Key]
    public int Id { get; set; }
   
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set;} = null!;

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
}
