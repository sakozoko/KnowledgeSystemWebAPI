using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AnswerDumpRepository : BaseRepository<AnswerDumpEntity>, IAnswerDumpRepository
{
    public AnswerDumpRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<AnswerDumpEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await DbContext.AnswerDumps
            .Include(a => a.Answer)
            .Include(a => a.Question)
            .ThenInclude(q => q!.Answers)
            .Include(a => a.PassedTest)
            .FirstOrDefaultAsync(ad => ad.Id == id, ct);
    }
}