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
    public virtual async Task<T?> GetByIdAsync(int id)
    => await DbContext.Set<T>().FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    =>  await DbContext.Set<T>().ToListAsync();

    public virtual async Task<int> AddAsync(T entity)
    {
        if(entity is null){
            throw new ArgumentNullException(nameof(entity));
        }
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbContext.Entry(entity).State= EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task<T> DeleteAsync(T entity)
    {
        DbContext.Entry(entity).State = EntityState.Deleted;
        await DbContext.SaveChangesAsync();
        return entity;
    }
}