using System.Security.Claims;
using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Extensions.Mappers;
using UserService.ViewModels;

namespace UserService.Features.Queries;

public record GetUsersQuery(ClaimsPrincipal User) : IRequest<IEnumerable<UserViewModel>>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserViewModel>>
    {
        private readonly UserManager<UserEntity> _userManager;

        public GetUsersQueryHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            if (!request.User.Identity!.IsAuthenticated)
            {
                throw new SecurityTokenException("User is not authenticated");
            }
                
            var user = await _userManager.FindByIdAsync(request.User.FindFirst("sub")!.Value);
            if(user == null)
            {
                throw new SecurityTokenException("User not found");
            }
            var userRole = await _userManager.GetRolesAsync(user);
            if(!userRole.Contains(Role.Admin))
            {
                throw new SecurityTokenException("User is not admin");
            }
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            return await users.ToViewModelsIncludeRolesAsync(_userManager);
        }
    }
}