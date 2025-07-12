using LMSWebAppClean.Application.Interface;
using MediatR;
using LMSWebAppClean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Domain.Base;

namespace LMSWebAppClean.Application.Usecase.Users.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<BaseUser>>, IQuery
    {
        public GetAllUsersQuery() {}
    }
}
