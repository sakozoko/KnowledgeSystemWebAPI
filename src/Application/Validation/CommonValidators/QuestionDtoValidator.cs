using Application.ViewModels;
using FluentValidation;

namespace Application.Validation.CommonValidators;

public class QuestionDtoValidator : AbstractValidator<QuestionDto>
{
    public QuestionDtoValidator()
    {
        RuleFor(q => q.Text)
            .NotEmpty()
            .MaximumLength(512);

        RuleFor(q => q.Mark)
            .Must(MarkIsPositive)
            .WithMessage("Mark must be positive");

        RuleFor(q => q.Answers)
            .NotEmpty()
            .Must(ea =>
                ea?.Any(a => a.IsCorrect) ?? false)
            .WithMessage("At least one answer must be correct");

        RuleFor(q => q).ChildRules(q =>
            q.RuleForEach(dto => dto.Answers)
                .SetValidator(new AnswerDtoValidator()));
    }

    //return false if the mark is initialized and the mark less than 0, if mark is noninitialized return true
    private static bool MarkIsPositive(decimal? mark)
    {
        return mark is null or >= 0;
    }
}