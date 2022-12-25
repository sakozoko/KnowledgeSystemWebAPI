using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    public Task<UserEntity?> GetByEmailAsync(string email);
    public Task<UserEntity?> GetByIdWithDetailsAsync(int id);
    public Task<IEnumerable<UserEntity?>> GetAllWithDetailsAsync();
}