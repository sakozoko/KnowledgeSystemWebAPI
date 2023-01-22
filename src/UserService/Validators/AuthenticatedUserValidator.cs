using System.Security.Claims;
using FluentValidation;

namespace UserService.Validators;

public class AuthenticatedUserValidator : AbstractValidator<ClaimsPrincipal?>
{
    public AuthenticatedUserValidator()
    {
        const string failureMsg = "User is not authenticated";
        RuleFor(x=>x)
            .NotNull()
            .WithMessage(failureMsg)
            .ChildRules(x=>x.RuleFor(c=>c!.Identity)
                .NotNull()
                .WithMessage(failureMsg)
                .ChildRules(v=>v.RuleFor(i=>i!.IsAuthenticated)
                    .Equal(true)
                    .WithMessage(failureMsg)));
    }
}