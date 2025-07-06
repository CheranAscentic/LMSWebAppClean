using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Users.CreateUser
{
    public class CreateUserCommand : IRequest<BaseUser>, ICommand
    {
        public int AuthId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }

        public CreateUserCommand(int authId, string username, string email, string password, string? fullName, string? role)
        {
            AuthId = authId;
            Username = username;
            Email = email;
            Password = password;
            FullName = fullName;
            Role = role;
        }
    }
}
