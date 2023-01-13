using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence;

public class IdentityServerDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{

    public IdentityServerDbContext()
    {
    }
    
    public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)
    {
    }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    
    

}