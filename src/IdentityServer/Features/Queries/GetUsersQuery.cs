using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using IdentityServer.Extensions.Mappers;
using IdentityServer.Validators;
using IdentityServer.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Features.Queries;

public record GetUsersQuery(ClaimsPrincipal User) : IRequest<IEnumerable<UserViewModel>>
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.User)
                .SetValidator(new AuthenticatedUserIsAdminValidator());
        }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserViewModel>>
    {
        private readonly UserManager<UserEntity> _userManager;

        public GetUsersQueryHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            return await users.ToViewModelsIncludeRolesAsync(_userManager);
        }
    }
}