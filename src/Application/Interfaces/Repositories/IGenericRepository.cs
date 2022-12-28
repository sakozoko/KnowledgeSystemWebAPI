using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    public Task<int> AddAsync(T entity, CancellationToken ct = default);
    public Task UpdateAsync(T entity, CancellationToken ct = default);
    public Task<T> DeleteAsync(int id, CancellationToken ct = default);
    public Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}