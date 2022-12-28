using Application.Extension.Repository;
using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Commands;

public class DeleteUserCommand : IRequest<UserEntity>
{
    public int Id { get; set; }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(IUserRepository userRepository)
        {
            RuleFor(d => d.Id).NotEmpty()
                .GreaterThan(0)
                .Must(userRepository.UserIsExist)
                .WithMessage("User must exist");
        }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserEntity>
        {
            private readonly IUserRepository _userRepository;

            public DeleteUserCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<UserEntity> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                return await _userRepository.DeleteAsync(request.Id, cancellationToken);
            }
        }
    }
}