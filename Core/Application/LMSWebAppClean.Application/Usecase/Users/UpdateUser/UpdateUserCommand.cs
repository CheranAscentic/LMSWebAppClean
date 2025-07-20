using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<BaseUser>, ICommand
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? address { get; set; }

        public UpdateUserCommand(int userId, string? name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
