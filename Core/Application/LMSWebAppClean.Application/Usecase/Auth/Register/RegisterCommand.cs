using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Auth.Register
{
    public class RegisterCommand : IRequest<BaseUser>, ICommand
    {
        public string Name { get; set; }
        public UserType Type { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public RegisterCommand(string name, UserType type, string email, string password)
        {
            Name = name;
            Type = type;
            Email = email;
            Password = password;
        }
    }
}
