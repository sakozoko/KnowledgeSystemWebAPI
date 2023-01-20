using Application.Extension.Mapping;
using Application.Interfaces.Repositories;
using Application.Models;
using Application.Validation.CommonValidators;
using Application.ViewModels;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public UserViewModel? User { get; set; }
    public int? Id { get; set; }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator(IUserRepository userRepository)
        {
            RuleFor(p => p.Id)
                .SetValidator(new EntityValidator<UserEntity>(userRepository));
            RuleFor(p => p.User)
                .ChildRules(p => p.RuleFor(u => u.Roles)
                    .Must(roles=>roles.Contains("Admin")));
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