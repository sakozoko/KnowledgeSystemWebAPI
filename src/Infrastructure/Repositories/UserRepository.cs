using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserEntity?> GetByIdAsync(int id)
        => await _dbContext.Users.FindAsync(id);

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
        => await _dbContext.Users.ToListAsync();
    

    public async Task<int> AddAsync(UserEntity entity)
    {
        if(entity is null){
            throw new ArgumentNullException(nameof(entity));
        }
        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(UserEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserEntity> DeleteAsync(UserEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    => await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<UserEntity?> GetByIdWithDetailsAsync(int id)
        => await _dbContext.Users
            .Include(c => c.Role)
            .Include(u => u.CreatedTests)
            .Include(u => u.PassedTests)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<UserEntity?>> GetAllWithDetailsAsync()
        => await _dbContext.Users
            .Include(c => c.Role)
            .Include(u => u.CreatedTests)
            .Include(u => u.PassedTests)
            .ToListAsync();
}