using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using IdentityServer.Exceptions.IdentityResultFailedException;
using IdentityServer.Validators;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Features.Commands;

public class UpdateUserInfoCommand : IRequest<IdentityResult>
{
    private string? Id { get; set; }
    private ClaimsPrincipal? User { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserName { get; set; }
    

    public void SetId(string id)
    {
        Id = id;
    }
    public void SetUser(ClaimsPrincipal? user)
    {
        User = user;
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserInfoCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .SetValidator(new ConvertableToGuidValidator());
            RuleFor(x => x.User)
                .SetValidator(new AuthenticatedUserValidator());
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserInfoCommand, IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public UpdateUserCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id!);
            if (user == null)
            {
                throw new IdentityResultFailedException(IdentityResultFailedCodes.UserNotFound);
            }
                
            if(request.Id!=request.User!.FindFirstValue(ClaimTypes.NameIdentifier) 
               && !request.User!.IsInRole(Role.Admin))
            {
                throw new IdentityResultFailedException(IdentityResultFailedCodes.AccessDenied);
            }
            return await UpdateUser(user, request);
        }

        private async Task<IdentityResult> UpdateUser(UserEntity user, UpdateUserInfoCommand request)
        {
            user.FirstName = request.FirstName ?? user.FirstName;
            user.SecondName = request.LastName ?? user.SecondName;
            user.UserName = request.UserName ?? user.UserName;
            user.Email = request.Email ?? user.Email;
            user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            

            return await _userManager.UpdateAsync(user);
        }
        
        


        
    }
}