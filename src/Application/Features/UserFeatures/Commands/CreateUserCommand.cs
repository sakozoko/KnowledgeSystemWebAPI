using Application.Extension.Repository;
using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Commands;

public class CreateUserCommand : IRequest<int>
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? RoleId { get; set; }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            RuleFor(c => c.Email).NotEmpty()
                .MaximumLength(320)
                .EmailAddress()
                .Must(userRepository.IsEmailUnique)
                .WithMessage("Email must be unique, length less than 320");
            RuleFor(c => c.Password).NotEmpty()
                .MaximumLength(128);
            RuleFor(c => c.UserName).NotEmpty()
                .MaximumLength(64)
                .Must(userRepository.IsUserNameUnique)
                .WithMessage("Username must be unique, length less than 64");
            RuleFor(c => c.FirstName).NotEmpty()
                .MaximumLength(64);
            RuleFor(c => c.Surname).NotEmpty()
                .MaximumLength(64);
            RuleFor(c => c.Phone).NotEmpty()
                .Matches("^[0-9]*$")
                .MaximumLength(15)
                .Must(userRepository.IsPhoneUnique)
                .WithMessage("Phone must be unique, length less than 15");
            RuleFor(c => c.RoleId)
                .SetValidator(new EntityValidator<RoleEntity>(roleRepository));
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId!.Value, cancellationToken);
            var user = new UserEntity
            {
                Email = request.Email,
                FirstName = request.FirstName,
                Password = request.Password,
                Phone = request.Phone,
                Role = role,
                Surname = request.Surname,
                UserName = request.UserName,
                CreatedDate = DateTime.Now
            };

            return await _userRepository.AddAsync(user, cancellationToken);
        }
    }
}