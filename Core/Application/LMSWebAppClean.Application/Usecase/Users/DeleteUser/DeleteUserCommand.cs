using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest<BaseUser>, ICommand
    {
        public int AuthId { get; set; }
        public int UserId { get; set; }

        public DeleteUserCommand(int authId, int userId)
        {
            AuthId = authId;
            UserId = userId;
        }
    }
}
