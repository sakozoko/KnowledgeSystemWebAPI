using Application.Extension.Repository;
using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Commands;

public class UpdateUserCommand : IRequest<int>
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? RoleId { get; set; }

    public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidation(IUserRepository userRepository, IRoleRepository roleRepository)
        {

            RuleFor(c => c.Id)
                .SetValidator(new EntityValidator<UserEntity>(userRepository));
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

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithDetailsAsync(request.Id.Value, cancellationToken);
            var role = await _roleRepository.GetByIdAsync(request.RoleId.Value, cancellationToken);

            user!.Role = role;
            user.CreatedDate = DateTime.Now;
            user.Email = request.Email;
            user.Password = request.Password;
            user.Phone = request.Phone;
            user.FirstName = request.FirstName;
            user.Surname = request.Surname;
            user.UserName = request.UserName;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return user.Id;
        }
    }
}