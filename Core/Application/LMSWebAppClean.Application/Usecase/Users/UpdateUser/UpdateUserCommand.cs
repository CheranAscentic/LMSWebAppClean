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
        public int AuthId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }

        public UpdateUserCommand(int authId, int userId, string? name)
        {
            AuthId = authId;
            UserId = userId;
            Name = name;
        }
    }
}
