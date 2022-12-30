namespace Application.ViewModels;

public class UserDto : BaseViewModel
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? RoleId { get; set; }
    public IEnumerable<PassedTestDto>? PassedTests { get; set; }
    public IEnumerable<TestDto>? CreatedTests { get; set; }
}