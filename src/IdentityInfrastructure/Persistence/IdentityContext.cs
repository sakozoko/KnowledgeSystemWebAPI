using IdentityInfrastructure.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Persistence;

public class IdentityContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{

    public IdentityContext()
    {
    }
    
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    
    

}