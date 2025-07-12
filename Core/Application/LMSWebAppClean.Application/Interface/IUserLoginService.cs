using LMSWebAppClean.Application.DTO.Identity;

namespace LMSWebAppClean.Application.Interface
{
    public interface IUserLoginService
    {
        Task<UserLoginResult> LoginAsync(string email, string password, bool rememberMe = false);
        Task LogoutAsync();
    }
}