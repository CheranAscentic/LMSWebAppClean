using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.GetUserById
{
    public class GetUserByIdQuery : IRequest<BaseUser>, IQuery
    {
        public int AuthId { get; set; }
        public int UserId { get; set; }

        public GetUserByIdQuery(int authId, int userId)
        {
            AuthId = authId;
            UserId = userId;
        }
    }
}
