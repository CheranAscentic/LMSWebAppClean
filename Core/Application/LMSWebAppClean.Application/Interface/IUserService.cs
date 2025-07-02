using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Interface
{
    public interface IUserService
    {
        public BaseUser AddUser(int authid, string name, UserType type);
        public BaseUser RemoveUser(int authid, int userId);
        public BaseUser GetUser(int authid, int userid);
        public BaseUser UpdateUser(int authid, int userid, string? name, UserType? type);
        public List<BaseUser> GetAllUsers(int authid);
    }
}
