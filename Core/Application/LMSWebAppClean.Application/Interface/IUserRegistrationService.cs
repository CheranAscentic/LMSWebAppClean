using LMSWebAppClean.Application.DTO.Identity;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.Application.Interface
{
    public interface IUserRegistrationService
    {
        Task<UserRegistrationResult> RegisterUserAsync(string name, string email, string password, string userType, CancellationToken cancellationToken = default);
        Task<bool> UserExistsAsync(string email);
    }
}