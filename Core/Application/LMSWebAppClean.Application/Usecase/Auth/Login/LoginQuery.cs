using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Auth.Login
{
    public class LoginQuery : IRequest<BaseUser>, IQuery
    {
        public int UserId { get; set; }

        public LoginQuery(int userId)
        {
            UserId = userId;
        }
    }
}
