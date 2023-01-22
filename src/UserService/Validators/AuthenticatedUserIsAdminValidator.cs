using System.Security.Claims;
using FluentValidation;
using IdentityInfrastructure.Model;

namespace UserService.Validators;

public class AuthenticatedUserIsAdminValidator : AbstractValidator<ClaimsPrincipal>
{
    public AuthenticatedUserIsAdminValidator()
    {
        RuleFor(x => x)
            .SetValidator(new AuthenticatedUserValidator());
        RuleFor(x =>
                x.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value))
            .Must(c => c.Contains(Role.Admin))
            .WithMessage("User is not authorized");
    }
}