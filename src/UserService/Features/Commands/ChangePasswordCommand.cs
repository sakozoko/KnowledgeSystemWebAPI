using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Exceptions.IdentityResultFailedException;

namespace UserService.Features.Commands;

public class ChangePasswordCommand : IRequest<IdentityResult>
{
    private ClaimsPrincipal? User { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    
    public void SetUser(ClaimsPrincipal user)
    {
        User = user;
    }
    
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c => c.Password).NotEmpty();
            RuleFor(c => c.ConfirmPassword).NotEmpty();
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password);
            RuleFor(c => c.OldPassword).NotEmpty();
        }
    }
    
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public ChangePasswordCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var getUserWithResult = await TryGetUser(request);
            if (!getUserWithResult.IdentityResult.Succeeded)
            {
                throw new IdentityResultFailedException(IdentityResultFailedCodes.UserNotFound);
            }

            var user = getUserWithResult.User!;
            var updatePasswordResult = await UpdatePasswordAsync(user, request);
            if (!updatePasswordResult.Succeeded)
            {
                throw new IdentityResultFailedException(IdentityResultFailedCodes.AccessDenied);
            }
            return updatePasswordResult;
        }
        
        private async Task<(UserEntity? User, IdentityResult IdentityResult)> TryGetUser(ChangePasswordCommand request)
        {
            UserEntity? user=default;
            var result = IdentityResult.Success;
            if (!string.IsNullOrEmpty(request.Email))
            {
                var getUserAndIdentityResult =
                    await GetUserAndIdentityResult(_userManager.FindByEmailAsync(request.Email));
                result = SetUserOrIdentityResultFailedIfNull(getUserAndIdentityResult, ref user);
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                var getUserAndIdentityResult =
                    await GetUserAndIdentityResult(_userManager.FindByNameAsync(request.UserName));
                result = SetUserOrIdentityResultFailedIfNull(getUserAndIdentityResult, ref user);
            }
            else if(result.Succeeded)
            {
                var getUserAndIdentityResult =
                    await GetUserAndIdentityResult(FindUserById(request));
                result = SetUserOrIdentityResultFailedIfNull(getUserAndIdentityResult, ref user);
            }

            return (user, result);

        }

        private IdentityResult SetUserOrIdentityResultFailedIfNull(
            (UserEntity? user, IdentityResult identityResult) tuple, ref UserEntity? userEntity)
        {
            if (!tuple.identityResult.Succeeded)
                return tuple.identityResult;
            userEntity = tuple.user;
            return tuple.identityResult;
        }

        private async Task<(UserEntity? User, IdentityResult IdentityResult)> GetUserAndIdentityResult(Task<UserEntity?> userFindingTask)
        {
            var user = await userFindingTask;
            if (user == null)
            {
                var typeOfUser = userFindingTask.GetType();
                return (user, IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = userFindingTask.GetType().FullName + " returned null"
                }));
            }

            return (user, IdentityResult.Success);
        }


        private async Task<UserEntity?> FindUserById(ChangePasswordCommand request)
        {
            UserEntity? user = default;
            var userId = request.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                user = await _userManager.FindByIdAsync(userId);
            }

            return user;
        }

        private async Task<IdentityResult> UpdatePasswordAsync(UserEntity user, ChangePasswordCommand request)
        {
            var result = IdentityResult.Success;
            if (ValidateChangingPassword(request.OldPassword, request.Password, request.ConfirmPassword))
            {
                result = await _userManager.ChangePasswordAsync(user, request.OldPassword!, request.Password!);
                if (!result.Succeeded) return result;
            }

            return result;
        }
        private static bool ValidateChangingPassword(string? oldPassword, string? password, string? confirmPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword)) return false;
            return password == confirmPassword;
        }
    }
}