namespace Application.ViewModels;

public record UserViewModel(Guid Id, IEnumerable<string> Roles);