using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserService.Features.Commands;

public class UpdateUserCommand : IRequest<IdentityResult>
{
    private string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? UserName { get; set; }
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Role { get; set; }
    public void SetId(string id) => Id = id;
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public UpdateUserCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        
        
        public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found"
                });
            }
            user.FirstName= request.FirstName ?? user.FirstName;
            user.SecondName = request.LastName ?? user.SecondName;
            await _userManager.SetEmailAsync(user, request.Email ?? user.Email);

            await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber ?? user.PhoneNumber);
            
            await _userManager.SetUserNameAsync(user, request.UserName ?? user.UserName);
            
            if (ValidateChangingPassword(request.OldPassword, request.Password, request.ConfirmPassword)){
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword!, request.Password!);
                if (!result.Succeeded)
                {
                    return result;
                }
            }
            if (request.Role != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            return await _userManager.UpdateAsync(user);
        }
        private static bool ValidateChangingPassword(string? oldPassword, string? password, string? confirmPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                return false;
            }
            return password == confirmPassword;
        }
    }
}