using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;

namespace LMSWebAppClean.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;

        public UserService(IUnitOfWork unitOfWork, IPermissionChecker permissionChecker, IRepository<BaseUser> userRepositories)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepositories));
        }

        public BaseUser AddUser(int authId, string name, UserType type)
        {
            permissionChecker.Check(authId, Permission.UserAdd, "User does not have permission to add user.");
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

        public List<BaseUser> GetAllUsers(int authId)
        {
            permissionChecker.Check(authId, Permission.UserViewAll, "User does not have permission to view all users.");
            return userRepository.GetAll();
        }

        public BaseUser GetUser(int authId, int userId)
        {
            permissionChecker.Check(authId, Permission.UserView, "User does not have permission to view user.");
            return userRepository.Get(userId) ?? throw new Exception("User with Title cannot be found");
        }

        public BaseUser UpdateUser(int authId, int userId, string? name, UserType? type)
        {
            permissionChecker.Check(authId, Permission.UserUpdate, "User does not have permission to update user.");
            BaseUser user = userRepository.Get(userId) ?? throw new Exception("User with Id cannot be found");
            user.Name = name ?? user.Name;
            user.Type = type ?? user.Type;
            var result = userRepository.Update(user);
            unitOfWork.SaveChanges();
            return result;
        }

        public BaseUser RemoveUser(int authId, int userId)
        {
            permissionChecker.Check(authId, Permission.UserDelete, "User does not have permission to delete user.");
            var result = userRepository.Remove(userId) ?? throw new Exception("User with Id cannot be found");
            unitOfWork.SaveChanges();
            return result;
        }
    }
}
