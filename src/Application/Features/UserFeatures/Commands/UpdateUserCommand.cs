using Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.UserFeatures.Commands;

public class UpdateUserCommand : IRequest<int>
{
    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }

    public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidation()
        {
            RuleFor(c => c.Id).NotEmpty().GreaterThan(0);
            RuleFor(c=>c.Email).NotEmpty().Matches("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$").MaximumLength(100);
            RuleFor(c=>c.Password).NotEmpty().MaximumLength(100);
            RuleFor(c=>c.UserName).NotEmpty().MaximumLength(100);
            RuleFor(c=>c.Name).NotEmpty().MaximumLength(100);
            RuleFor(c=>c.Surname).NotEmpty().MaximumLength(100);
            RuleFor(c=>c.Phone).NotEmpty().Matches("^[0-9]*$").MaximumLength(20);
            RuleFor(c=>c.Role).NotEmpty().MaximumLength(30);
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        
        public UpdateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithDetailsAsync(request.Id.Value);
            if (user is null)
            {
                return default;
            }
            var role = await _roleRepository.GetRoleByNameAsync(request.Role);
            if (role is null)
            {
                return default;
            }

            user.Role = role;
            user.CreatedDate = DateTime.Now;
            user.Email = request.Email;
            user.Password = request.Password;
            user.Phone = request.Phone;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.UserName = request.UserName;
            

            return user.Id;
        }
    } 
}