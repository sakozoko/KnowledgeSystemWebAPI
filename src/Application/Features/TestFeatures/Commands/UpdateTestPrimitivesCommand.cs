using Application.Interfaces.Repositories;
using Application.Validation.CommonValidators;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Features.TestFeatures.Commands;

public class UpdateTestPrimitivesCommand : IRequest<int>
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public class UpdateTestCommandValidator : AbstractValidator<UpdateTestPrimitivesCommand>
    {
        public UpdateTestCommandValidator(ITestRepository testRepository)
        {
            RuleFor(t => t.Id)
                .SetValidator(new EntityValidator<TestEntity>(testRepository));

            RuleFor(t => t.Title)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(t => t.Description)
                .MaximumLength(512);
        }
    }

    public class UpdateTestCommandHandler : IRequestHandler<UpdateTestPrimitivesCommand, int>
    {
        private readonly ITestRepository _testRepository;

        public UpdateTestCommandHandler(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<int> Handle(UpdateTestPrimitivesCommand request, CancellationToken cancellationToken)
        {
            var test = await _testRepository.GetByIdAsync(request.Id!.Value, cancellationToken);
            test!.Title = request.Title!;
            test.Description = request.Description;
            test.LastModifiedDate = DateTime.Now;
            await _testRepository.UpdateAsync(test, cancellationToken);
            return test.Id;
        }
    }
}