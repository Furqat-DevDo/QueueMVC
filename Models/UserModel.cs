namespace Queue.Models;

public class UserModel : IdentityUser
{
    [Required]
    [MaxLength(64)]
    public string Fullname { get; set; }

    [Required]
    public DateTimeOffset Birthdate { get; set; }
    
    public string Password { get; set; }
}
