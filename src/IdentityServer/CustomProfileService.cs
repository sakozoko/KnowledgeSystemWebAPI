using System.Security.Claims;
using IdentityInfrastructure.Model;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer;

public class CustomProfileService : IProfileService
{
    private readonly ILogger<CustomProfileService> _logger;
    private readonly IUserClaimsPrincipalFactory<UserEntity> _userClaimsPrincipalFactory;
    private readonly UserManager<UserEntity> _userManager;

    public CustomProfileService(
        ILogger<CustomProfileService> logger,
        IUserClaimsPrincipalFactory<UserEntity> userClaimsPrincipalFactory,
        UserManager<UserEntity> userManager
    )
    {
        _logger = logger;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _userManager = userManager;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subjectId = context.Subject.GetSubjectId();
        var user = await FindUserBySubjectIdAsync(subjectId);
        var claimsPrincipal = await  _userClaimsPrincipalFactory.CreateAsync(user!);
        context.AddRequestedClaims(claimsPrincipal.Claims);
        //adding custom properties
        context.AddRequestedClaims(GetUserCustomClaims(user));
    }
    
    private static IEnumerable<Claim> GetUserCustomClaims(UserEntity? user)
    {
        const int maxCustomClaims = 2;
        var claims = new List<Claim>(maxCustomClaims);
        if(user?.FirstName != null)
            claims.Add(new Claim("FirstName", user.FirstName));
        if(user?.SecondName != null)
            claims.Add(new Claim("SecondName", user.SecondName));
        return claims;
    }

    private async Task<UserEntity?> FindUserBySubjectIdAsync(string subjectId)
    {
        var user = await _userManager.FindByIdAsync(subjectId);
        if(user == null)
        {
            _logger.LogWarning("No user found matching subject ID: {SubjectId}", subjectId);
        }

        return user;

    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subjectId = context.Subject.GetSubjectId();
        var applicationUser = await _userManager.FindByIdAsync(subjectId);
        context.IsActive = applicationUser != null;
    }
    
    
    
}