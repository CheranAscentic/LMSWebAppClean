using LMSWebAppClean.Application.DTO;
using LMSWebAppClean.Application.Interface;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Identity.LoginUser
{
    public class LoginUserCommand : IRequest<LoginDTO> , ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginUserCommand(string email, string password, bool rememberMe = false)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            RememberMe = rememberMe;
        }
    }
}