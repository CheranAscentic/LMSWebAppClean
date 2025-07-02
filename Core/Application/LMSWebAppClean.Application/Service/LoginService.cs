using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;


        public LoginService(IUnitOfWork unitOfWork, IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public BaseUser Login(/*int authId, */int userId)
        {
            // No permission check for user login
            //permissionChecker.Check(authId, Permission.Loginlogin, "User does not have permission to login.");
            return userRepository.Get(userId) ?? throw new Exception("User with ID cannot be found to login");
        }

        public BaseUser Regsiter(/*int authId,*/string name, UserType type)
        {
            // No Permission check for user registration
            BaseUser newUser;

            switch (type)
            {
                case UserType.Member:
                    newUser = new Member(name);
                    break;
                case UserType.StaffMinor:
                    newUser = new Staff(name, type);
                    break;
                case UserType.StaffManagement:
                    newUser = new Staff(name, type);
                    break;
                default:
                    throw new ArgumentException("Invalid user type");
            }

            var result = userRepository.Add(newUser);
            unitOfWork.SaveChanges();
            return result;
        }
    }
}
