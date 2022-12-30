using Application.Extension.Repository;
using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Application.ViewModels;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TestFeatures.Commands;

public class UpdateTestMaxMarkAndQuestions : IRequest<int>
{
    public int? TestId { get; set; }
    public decimal? MaxMark { get; set; }
    public IEnumerable<QuestionDto>? Questions { get; set; }

    public class UpdateTestMaxMarkAndQuestionsValidator : AbstractValidator<UpdateTestMaxMarkAndQuestions>
    {
        public UpdateTestMaxMarkAndQuestionsValidator(ITestRepository testRepository)
        {
            RuleFor(t => t.TestId)
                .SetValidator(new EntityValidator<TestEntity>(testRepository));

            RuleFor(t => t.MaxMark)
                .NotEmpty()
                .Must((command, newMaxMark) =>
                    testRepository.NewMaxMarkEqualsSumOfQuestionMarks(command.TestId!.Value, newMaxMark!.Value))
                .WithMessage("New max mark must be equal to sum of question marks");

            RuleForEach(t => t.Questions)
                .SetValidator(new QuestionDtoValidator());
        }
    }
}