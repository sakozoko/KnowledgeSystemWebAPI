using Application.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;

namespace Application.Validation.CommonValidators;

public class EntityValidator<T>  : AbstractValidator<int?> where T: BaseEntity
{
    public EntityValidator(IGenericRepository<T> repository)
    {
        RuleFor(x => x)
            .NotEmpty();
        RuleFor(x => x!.Value)
            .GreaterThan(0);
        RuleFor(x=>x!.Value)
            .Must(c=>repository.IsExistsAsync(c).Result)
                .WithMessage("Entity not found");
    }
}

