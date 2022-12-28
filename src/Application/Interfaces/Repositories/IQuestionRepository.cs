using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuestionRepository : IGenericRepository<QuestionEntity>
{
    public Task<IEnumerable<QuestionEntity>> GetAllWithDetailsAsync(CancellationToken ct = default);
}