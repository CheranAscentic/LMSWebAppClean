using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.DTO.Auth;

namespace LMSWebAppClean.Application.DTO.Identity
{
    public class UserLoginResult
    {
        public bool IsSuccess { get; set; }
        public LoginDTO? loginDTO { get; set; }
        public string? ErrorMessage { get; set; }

        public static UserLoginResult Success(LoginDTO loginDTO)
        {
            return new UserLoginResult
            {
                IsSuccess = true,
                loginDTO = loginDTO
            };
        }

        public static UserLoginResult Failure(string errorMessage)
        {
            return new UserLoginResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}