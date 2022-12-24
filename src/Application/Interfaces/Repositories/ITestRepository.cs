using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITestRepository : IGenericRepository<TestEntity>
{
    public Task<IQueryable<TestEntity>> GetAllWithDetailsAsync();
}