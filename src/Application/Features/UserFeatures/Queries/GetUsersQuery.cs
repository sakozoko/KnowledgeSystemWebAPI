using Application.Extension.Mapping;
using Application.Interfaces.Repositories;
using Application.ViewModels;
using MediatR;

namespace Application.Features.UserFeatures.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users= await _userRepository.GetAllWithDetailsAsync(cancellationToken);
            return users.ToUserDtos();
        }
    }
}