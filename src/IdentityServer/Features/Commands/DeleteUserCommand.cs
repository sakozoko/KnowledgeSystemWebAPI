using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using IdentityServer.Exceptions.IdentityResultFailedException;
using IdentityServer.Validators;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Commands;

public record DeleteUserCommand(string? Id, ClaimsPrincipal User) : IRequest<IdentityResult>
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => 
                    x.Id)
                .SetValidator(new ConvertableToGuidValidator());
            RuleFor(x => x.User)
                .SetValidator(new AuthenticatedUserIsAdminValidator());
        }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public DeleteUserCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id!);
            if (user == null)
                throw new IdentityResultFailedException(IdentityResultFailedCodes.UserNotFound);
            return await _userManager.DeleteAsync(user);
        }
    }
}