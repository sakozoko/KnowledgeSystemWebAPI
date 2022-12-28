using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext DbContext;

    protected BaseRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await DbContext.Set<T>().FindAsync(new object[] { id }, ct);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbContext.Set<T>().ToListAsync(ct);
    }

    public virtual async Task<int> AddAsync(T entity, CancellationToken ct = default)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        await DbContext.Set<T>().AddAsync(entity, ct);
        await DbContext.SaveChangesAsync(ct);
        return entity.Id;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(ct);
    }

    public virtual async Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate,
        CancellationToken ct = default)
    {
        return await DbContext.Set<T>().FirstOrDefaultAsync(predicate, ct);
    }

    public virtual async Task<T> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await DbContext.Set<T>().FindAsync(new object[] { id }, ct);
        if (entity is null)
            throw new ArgumentException("Entity not found", nameof(id));
        DbContext.Entry(entity).State = EntityState.Deleted;
        await DbContext.SaveChangesAsync(ct);
        return entity;
    }
}