using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IRoleRepository : IGenericRepository<RoleEntity>
{
    public Task<RoleEntity?> GetRoleByNameAsync(string name,CancellationToken ct=default);
}