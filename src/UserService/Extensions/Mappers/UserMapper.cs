using IdentityInfrastructure.Model;
using Microsoft.AspNetCore.Identity;
using UserService.ViewModels;

namespace UserService.Extensions.Mappers;

public static class UserMapper
{
    public static UserViewModel ToViewModel(this UserEntity user)
    {
        return user.ToViewModel(Array.Empty<string>());
    }

    public static UserViewModel ToViewModel(this UserEntity user, string role)
    {
        return user.ToViewModel(new[] { role });
    }

    public static UserViewModel ToViewModel(this UserEntity user, IEnumerable<string> roles)
    {
        return new UserViewModel
        {
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.SecondName,
            PhoneNumber = user.PhoneNumber,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            Roles = roles
        };
    }

    public static IEnumerable<UserViewModel> ToViewModels(this IEnumerable<UserEntity> users)
    {
        return users.Select(ToViewModel);
    }

    public static async Task<IEnumerable<UserViewModel>> ToViewModelsIncludeRolesAsync(
        this IEnumerable<UserEntity> users, UserManager<UserEntity> userManager)
    {
        var result = new List<UserViewModel>();

        foreach (var userEntity in users)
        {
            var roles = await userManager.GetRolesAsync(userEntity);
            result.Add(userEntity.ToViewModel(roles));
        }

        return result;
    }
}