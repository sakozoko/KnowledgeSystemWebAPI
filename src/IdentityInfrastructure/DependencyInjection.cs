using IdentityInfrastructure.Model;
using IdentityInfrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=identityServer;Trusted_Connection=True;MultipleActiveResultSets=true"));

        services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
        })
            .AddEntityFrameworkStores<IdentityContext>();

        return services;
    }

}