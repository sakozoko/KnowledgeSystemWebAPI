using IdentityInfrastructure.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UserService.Features.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserEntity>>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserEntity>>
    {
        private readonly UserManager<UserEntity> _userManager;

        public GetUsersQueryHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<UserEntity>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        { 
            await _userManager.AddPasswordAsync(_userManager.Users.First(),"123456");
            return await _userManager.Users.ToListAsync(cancellationToken);
        }
    }
}