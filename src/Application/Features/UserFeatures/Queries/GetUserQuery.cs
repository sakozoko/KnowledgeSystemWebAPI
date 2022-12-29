using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Queries;

public class GetUserQuery : IRequest<UserEntity?>
{
    public int Id { get; set; }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {         
            RuleFor(p => p.Id).NotEmpty()
                .GreaterThan(0);
        }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserEntity?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity?> Handle(GetUserQuery request, CancellationToken cancellationToken)
            => await _userRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        
    }

}