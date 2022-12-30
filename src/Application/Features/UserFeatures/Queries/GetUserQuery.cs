using Application.Extension.Mapping;
using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Application.ViewModels;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public int? Id { get; set; }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator(IUserRepository userRepository)
        {
            RuleFor(p => p.Id)
                .SetValidator(new EntityValidator<UserEntity>(userRepository));
        }
    }
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithDetailsAsync(request.Id!.Value, cancellationToken);
            return user!.ToUserDto();
        } 

        
    }

}