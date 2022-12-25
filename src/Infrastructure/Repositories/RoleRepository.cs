using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : BaseRepository<RoleEntity>,IRoleRepository
{

    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
    public async Task<RoleEntity?> GetRoleByNameAsync(string name)
    {
        return await DbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}