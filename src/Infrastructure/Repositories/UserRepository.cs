using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await DbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<UserEntity?> GetByUserNameAsync(string username, CancellationToken ct = default)
    {
        return await DbContext.Users.FirstOrDefaultAsync(u => u.UserName == username, ct);
    }

    public async Task<UserEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
    {
        return await DbContext.Users
            .Include(c => c.Role)
            .Include(u => u.CreatedTests)
            .Include(u => u.PassedTests)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<IEnumerable<UserEntity>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await DbContext.Users
            .Include(c => c.Role)
            .Include(u => u.CreatedTests)
            .Include(u => u.PassedTests)
            .ToListAsync(ct);
    }
}