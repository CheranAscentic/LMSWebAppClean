using LMSWebAppClean.Domain.Enum;
using MediatR;
using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Identity.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>, ICommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public RegisterUserCommand(string name, string email, string password, string userType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            UserType = userType;
        }
    }
}