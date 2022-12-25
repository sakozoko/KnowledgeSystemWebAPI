using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<int> AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task<T> DeleteAsync(T entity);
}