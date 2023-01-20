using Microsoft.AspNetCore.Identity;

namespace UserService.Entity;

public class UserEntity : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
}