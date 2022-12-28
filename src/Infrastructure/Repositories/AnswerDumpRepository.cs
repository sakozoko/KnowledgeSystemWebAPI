using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnswerDumpRepository : BaseRepository<AnswerDumpEntity>, IAnswerDumpRepository
{
    public AnswerDumpRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<AnswerDumpEntity?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await DbContext.AnswerDumps
            .Include(ad => ad.Answer)
            .ThenInclude(a => a.Question)
            .ThenInclude(q => q.Answers)
            .Include(ad => ad.Answer)
            .ThenInclude(a => a.Question)
            .ThenInclude(q => q.Test)
            .ThenInclude(t => t.UserCreator)
            .Include(ad => ad.Answer)
            .ThenInclude(a => a.Question)
            .ThenInclude(q => q.Test)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .ThenInclude(a => a.Question)
            .Include(ad => ad.PassedTest)
            .ThenInclude(pt => pt.User)
            .Include(ad => ad.PassedTest)
            .ThenInclude(pt => pt.Test)
            .Include(ad => ad.PassedTest)
            .ThenInclude(pt => pt.Answers)
            .FirstOrDefaultAsync(ad => ad.Id == id, ct);
    }
}