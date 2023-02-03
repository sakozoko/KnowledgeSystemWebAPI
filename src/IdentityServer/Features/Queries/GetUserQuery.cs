using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using IdentityServer.Exceptions.IdentityResultFailedException;
using IdentityServer.Extensions.Mappers;
using IdentityServer.Validators;
using IdentityServer.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Queries;

public record GetUserQuery(string? Id,ClaimsPrincipal User) : IRequest<UserViewModel?>
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.Id)
                .SetValidator(new ConvertableToGuidValidator());
            RuleFor(x => x.User)
                .SetValidator(new AuthenticatedUserValidator());
        }
    }
    
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel?>
    {
        private readonly UserManager<UserEntity> _userManager;

        public GetUserQueryHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<UserViewModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id!);
            if(user == null)
            {
                throw new IdentityResultFailedException(IdentityResultFailedCodes.UserNotFound);
            }
            UserViewModel? result;
            if (await _userManager.IsInRoleAsync(user, Role.Admin))
            {
                result = await GetUserViewModelForAdminAsync(request.Id!);
            }
            else
            {
                result = await GetUserViewModelForUserAsync(request.Id!);
            }

            return result;
        }

        private async Task<UserViewModel?> GetUserViewModelForUserAsync(string requestId)
        {
            var user = await GetUserEntity(requestId);
            return user?.ToViewModel();
        }


        private async Task<UserViewModel?> GetUserViewModelForAdminAsync(string requestId)
        {
            var user = await GetUserEntity(requestId);
            IList<string>? roles = default;
            if (user != null)
            {
                roles = await _userManager.GetRolesAsync(user);
            }
            return user?.ToViewModel(roles);
        }
        private async Task<UserEntity?> GetUserEntity(string requestId)
        {
            var user = await _userManager.FindByIdAsync(requestId);
            return user;
        }
    }


}