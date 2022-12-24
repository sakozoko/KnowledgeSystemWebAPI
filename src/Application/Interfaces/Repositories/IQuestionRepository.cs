using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IQuestionRepository : IGenericRepository<QuestionEntity>
{
    public Task<IQueryable<QuestionEntity>> GetAllWithDetailsAsync();
}