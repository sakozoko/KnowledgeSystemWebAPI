using FluentValidation;

namespace UserService.Validators;

public class ConvertableToGuidValidator : AbstractValidator<string?>
{
    public ConvertableToGuidValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("'{PropertyName}' must be a valid GUID.");
    }
}