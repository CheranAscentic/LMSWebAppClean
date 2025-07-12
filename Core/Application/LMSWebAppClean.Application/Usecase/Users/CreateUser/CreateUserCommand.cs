using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Users.CreateUser
{
    public class CreateUserCommand : IRequest<BaseUser>, ICommand
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; } = string.Empty;
        public int? Id { get; set; } // Optional ID field

        public CreateUserCommand(string? fullName, string type, string? email, int? id = null)
        {
            Name = fullName;
            Type = type;
            Email = email;
            Id = id;
        }
    }
}
