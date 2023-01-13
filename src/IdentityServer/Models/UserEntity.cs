using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class UserEntity : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public IdentityRole<Guid>? Role { get; set; }
}