using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuestionRepository : BaseRepository<QuestionEntity>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<QuestionEntity>> GetAllWithDetailsAsync(CancellationToken ct=default)
        => await DbContext.Questions
            .Include(q => q.Test)
            .Include(q => q.Answers)
            .ToListAsync(ct);
}