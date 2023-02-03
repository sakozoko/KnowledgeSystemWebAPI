using System.Reflection;
using FluentValidation;
using IdentityInfrastructure.Model;

namespace IdentityServer.Validators;

public class RoleIsExistValidator : AbstractValidator<string?>
{
    public RoleIsExistValidator()
    {
        var roleType = typeof(Role);
        var roleConstFields = roleType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        var roleConstValues = roleConstFields.Select(x => x.GetValue(null)).OfType<string>().ToList();
        RuleFor(c=>c)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(x=>roleConstValues.Contains(x!))
            .WithMessage("Role is not exist");
            
    }
}