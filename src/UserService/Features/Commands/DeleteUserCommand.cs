using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace UserService.Features.Commands;

public record DeleteUserCommand(string Id) : IRequest<IdentityResult>
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,IdentityResult>
    {
        private readonly UserManager<UserEntity> _userManager;

        public DeleteUserCommandHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            return await _userManager.DeleteAsync(user);
        }
    }
}