using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TestRepository : BaseRepository<TestEntity>, ITestRepository
{
    public TestRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<TestEntity>> GetAllWithDetailsAsync()
        => await DbContext.Tests
            .Include(t => t.UserCreator)
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .ToListAsync();

}