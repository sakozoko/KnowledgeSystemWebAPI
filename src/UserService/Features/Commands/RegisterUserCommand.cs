using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserService.Features.Commands;

public class RegisterUserCommand : IRequest<IdentityResult>
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<UserEntity> _userManager;

        public RegisterUserCommandHandler(UserManager<UserEntity> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new UserEntity
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                SecondName = request.LastName,
                Email = request.Email
            };
            await _userManager.CreateAsync(newUser, request.Password!);
            return await _userManager.AddToRoleAsync(newUser, "User");
        }
    }
}