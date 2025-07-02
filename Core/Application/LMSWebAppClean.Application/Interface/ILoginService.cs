using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Interface
{
    public interface ILoginService
    {
        BaseUser Login(/*int authId, */int userId);
        BaseUser Regsiter(/*int authId, */string name, UserType type);
    }
}
