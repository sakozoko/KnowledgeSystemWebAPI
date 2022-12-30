using Application.Interfaces.Repositories;

namespace Application.Extension.Repository;

public static class UserCommandValidations
{
    public static bool IsEmailUnique(this IUserRepository userRepository, string email)
    {
        return userRepository.GetByEmailAsync(email).Result is null;
    }

    public static bool IsUserNameUnique(this IUserRepository userRepository, string username)
    {
        return userRepository.GetByUserNameAsync(username).Result is null;
    }

    public static bool IsPhoneUnique(this IUserRepository userRepository, string phone)
    {
        return userRepository.GetByPredicateAsync(c => c.Phone == phone).Result is null;
    }
}