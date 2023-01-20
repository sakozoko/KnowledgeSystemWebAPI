using Microsoft.AspNetCore.Identity;

namespace IdentityInfrastructure.Model;

public class UserEntity : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
}