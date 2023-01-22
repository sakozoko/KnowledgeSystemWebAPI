using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Extensions.Mappers;
using UserService.ViewModels;

namespace UserService.Features.Queries;

public record GetUsersQuery(ClaimsPrincipal User) : IRequest<IEnumerable<UserViewModel>>
{

    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => 
                    x.User.Identity)
                .NotNull()
                .ChildRules(c=>
                    c.RuleFor(i=>i!.IsAuthenticated)
                        .Equal(true))
                .WithMessage("User is not authenticated");
            RuleFor(x => 
                    x.User.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value))
                .Must(c => c.Contains(Role.Admin))
                .WithMessage("User is not authorized");

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