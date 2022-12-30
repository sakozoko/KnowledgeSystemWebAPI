using Application.Extension.Mapping;
using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Application.ViewModels;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TestFeatures.Commands;

public class CreateTestCommand : IRequest<int>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ICollection<QuestionDto>? Questions { get; set; }
    public decimal? MaxMark { get; set; }
    public int? UserCreatorId { get; set; }

    public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestCommandValidator(IUserRepository userRepository)
        {
            RuleFor(t => t.Title)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(t => t.Description)
                .MaximumLength(512);

            RuleForEach(t => t.Questions)
                .NotEmpty()
                .ChildRules(questions =>
                    questions.RuleFor(q => q.Answers)
                        .NotEmpty()
                        .Must(answers =>
                            answers!.Any(a => a.IsCorrect))
                        .ForEach(answer =>
                            answer.ChildRules(answerDto =>
                                answerDto.RuleFor(a => a.Text)
                                    .NotEmpty()
                                    .MaximumLength(512))));

            RuleFor(t => t.MaxMark)
                .GreaterThanOrEqualTo(0)
                .Must(MaxMarkValueEqualsSumOfQuestionMarks)
                .WithMessage("Max mark must be greater than 0 or equal to 0, " +
                             "and sum of all questions marks must be equal to max mark");

            RuleFor(t => t.UserCreatorId)
                .SetValidator(new EntityValidator<UserEntity>(userRepository));
        }

        private static bool MaxMarkValueEqualsSumOfQuestionMarks(CreateTestCommand command, decimal? maxMark)
        {
            return command.Questions!.Sum(q => q.Mark ?? 0M) == maxMark;
        }
    }


    public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, int>
    {
        private readonly ITestRepository _testRepository;
        private readonly IUserRepository _userRepository;

        public CreateTestCommandHandler(ITestRepository testRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _testRepository = testRepository;
        }

        public async Task<int> Handle(CreateTestCommand command, CancellationToken cancellationToken)
        {
            var test = new TestEntity
            {
                Title = command.Title,
                Description = command.Description,
                CreatedDate = DateTime.Now,
                MaxMark = command.MaxMark,
                Questions = command.Questions!.ToQuestionsEntities().ToList(),
                UserCreator = await _userRepository.GetByIdAsync(command.UserCreatorId!.Value, cancellationToken),
                LastModifiedDate = DateTime.Now
            };

            return await _testRepository.AddAsync(test, cancellationToken);
        }
    }
}