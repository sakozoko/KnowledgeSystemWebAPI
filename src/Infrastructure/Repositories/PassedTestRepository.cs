using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class PassedTestRepository : BaseRepository<PassedTestEntity>, IPassedTestRepository
{
    public PassedTestRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}