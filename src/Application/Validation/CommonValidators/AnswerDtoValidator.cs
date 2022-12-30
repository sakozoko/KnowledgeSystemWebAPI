using FluentValidation;

namespace Application.Validation.CommonValidators;

public class AnswerDtoValidator : AbstractValidator<ViewModels.AnswerDto>
{
    public AnswerDtoValidator()
    {
        RuleFor(a => a.Text)
            .NotEmpty()
            .MaximumLength(512);
    }
}