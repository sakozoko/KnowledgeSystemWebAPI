namespace Domain.Entities;

public class UserEntity : BaseEntity
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public RoleEntity? Role { get; set; }
    public ICollection<PassedTestEntity>? PassedTests { get; set; }
    public ICollection<TestEntity>? CreatedTests { get; set; }
}