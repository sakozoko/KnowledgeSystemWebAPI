using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class RoleRepository : BaseRepository<RoleEntity>,IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<RoleEntity?> GetRoleByNameAsync(string name, CancellationToken ct = default)
        => await GetByPredicateAsync(r => r.Name == name, ct);
}