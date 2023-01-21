namespace UserService.ViewModels;

public class UserViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? UserName { get; set; }
    public IEnumerable<string> Roles { get; set; }

}