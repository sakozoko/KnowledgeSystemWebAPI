using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class AnswerRepository : BaseRepository<AnswerEntity>, IAnswerRepository
{
    public AnswerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
}