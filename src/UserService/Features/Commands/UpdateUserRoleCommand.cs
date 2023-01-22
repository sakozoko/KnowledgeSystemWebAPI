using FluentValidation;
using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Validators;

namespace UserService.Features.Commands;

public record UpdateUserRoleCommand(string? Id, string? Role) : IRequest<IdentityResult>
{

    public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleValidator()
        {
            RuleFor(c => c.Id)
                .SetValidator(new ConvertableToGuidValidator());
            RuleFor(c => c.Role)
                .SetValidator(new RoleIsExistValidator());
        }
    }

    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public UpdateUserRoleCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id!);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() 
                { 
                    Code = "UserNotFound",
                    Description = "User not found" 
                });
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.First();
            if (request.Role != null && role != IdentityInfrastructure.Model.Role.Admin)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
                return await _userManager.AddToRoleAsync(user, request.Role);
            }

            return IdentityResult.Success;
        }
    }
}