using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.UserFeatures.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserEntity>>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserEntity>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserEntity>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllWithDetailsAsync(cancellationToken);
        }
    }
}