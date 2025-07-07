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
        public int AuthId { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }

        public CreateUserCommand(int authId, string? fullName, UserType type)
        {
            AuthId = authId;
            Name = fullName;
            Type = type;
        }
    }
}
