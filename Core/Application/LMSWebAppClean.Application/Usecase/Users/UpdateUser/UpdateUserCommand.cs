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
        public string Username { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }

        public UpdateUserCommand(int authId, int userId, string username, string email, string? fullName, string? role)
        {
            AuthId = authId;
            UserId = userId;
            Username = username;
            Email = email;
            FullName = fullName;
            Role = role;
        }
    }
}
