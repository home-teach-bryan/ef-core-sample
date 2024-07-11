using EFCoreSample.Models;

namespace EFCoreSample.Services;

public interface IUserService
{
    (bool isValid, User user) IsValid(string name, string password);
}