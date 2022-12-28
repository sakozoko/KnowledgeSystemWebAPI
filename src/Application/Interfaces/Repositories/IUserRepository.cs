using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken ct=default);
    public Task<UserEntity?> GetByUserNameAsync(string name, CancellationToken ct = default);
    public Task<UserEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct=default);
    public Task<IEnumerable<UserEntity>> GetAllWithDetailsAsync(CancellationToken ct=default);
}