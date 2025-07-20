using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System.Collections.Generic;
using System.Linq;

namespace LMSWebAppClean.Application.PermissionChecker
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly Dictionary<string, List<string>> userTypePermissions;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public PermissionChecker(IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;

            this.userTypePermissions = new Dictionary<string, List<string>>
            {
                {
                    UserType.None,
                    new List<string>
                    {
                        Permission.HandleGetAllBooks,
                        Permission.HandleGetBookById,
                        Permission.Public,
                    }
                },
                {
                    UserType.Member,
                    new List<string>
                    {
                        Permission.HandleGetAllBooks,
                        Permission.HandleGetBookById,
                        Permission.Self.GetUserById,
                        Permission.Self.UpdateUser,
                        Permission.Self.BorrowBook,
                        Permission.Self.ReturnBook,
                        Permission.Self.GetBorrowedBooks,
                    }
                },
                {
                    UserType.StaffMinor,
                    new List<string>
                    {
                        Permission.HandleGetAllBooks,
                        Permission.HandleGetBookById,
                        Permission.Self.GetUserById,
                        Permission.Self.UpdateUser,
                        Permission.Process.UpdateBook,
                        Permission.Process.CreateBook,
                        Permission.Process.DeleteBook,
                        Permission.Process.BorrowBook,
                        Permission.Process.ReturnBook,
                        Permission.Process.GetBorrowedBooks,
                    }
                },
                {
                    UserType.StaffManagement,
                    new List<string>
                    {
                        Permission.Process.GetAllUsers,
                        Permission.HandleGetAllBooks,
                        Permission.HandleGetBookById,
                        Permission.Self.GetUserById,
                        Permission.Self.UpdateUser,
                        Permission.Process.UpdateUser,
                        Permission.Process.CreateUser,
                        Permission.Process.DeleteUser,
                        Permission.Process.CreateBook,
                        Permission.Process.UpdateBook,
                        Permission.Process.BorrowBook,
                        Permission.Process.DeleteBook,
                        Permission.Process.ReturnBook,
                        Permission.Process.GetBorrowedBooks,
                    }
                },
            };
        }

        public void Check(int requestUserId, int targetUserId, string selfPermission, string processPermission)
        {
            try
            {
                var requestUser = GetUser(requestUserId);
                var targetUser = GetUser(targetUserId);
                
                CheckUserAccess(requestUser, targetUser, selfPermission, processPermission);
            }
            catch (System.Exception ex) when (!(ex is System.UnauthorizedAccessException))
            {
                throw new System.UnauthorizedAccessException("Access denied due to invalid user data.");
            }
        }

        public void Check(BaseUser requestUser, BaseUser targetUser, string selfPermission, string processPermission)
        {
            if (requestUser == null)
                throw new System.UnauthorizedAccessException("Requesting user not found.");
            
            if (targetUser == null)
                throw new System.UnauthorizedAccessException("Target user not found.");
                
            CheckUserAccess(requestUser, targetUser, selfPermission, processPermission);
        }

        // Public Check methods for simple permission checks that throw UnauthorizedAccessException
        public void Check(int userId, string permission, string errorMessage)
        {
            try
            {
                var user = GetUser(userId);
                Check(user, permission, errorMessage);
            }
            catch (System.Exception ex) when (!(ex is System.UnauthorizedAccessException))
            {
                throw new System.UnauthorizedAccessException(errorMessage);
            }
        }

        public void Check(BaseUser user, string permission, string errorMessage)
        {
            if (user == null)
                throw new System.UnauthorizedAccessException(errorMessage);
                
            if (!HasPermission(user, permission))
                throw new System.UnauthorizedAccessException(errorMessage);
        }

        // Public CheckIf methods for user-to-user permission checks that return boolean
        public bool CheckIf(int requestUserId, int targetUserId, string selfPermission, string processPermission)
        {
            try
            {
                Check(requestUserId, targetUserId, selfPermission, processPermission);
                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }
        }

        public bool CheckIf(BaseUser requestUser, BaseUser targetUser, string selfPermission, string processPermission)
        {
            try
            {
                Check(requestUser, targetUser, selfPermission, processPermission);
                return true;
            }
            catch (System.UnauthorizedAccessException)
            {
                return false;
            }
        }

        // Public CheckIf methods for simple permission checks that return boolean
        public bool CheckIf(int userId, string permission)
        {
            try
            {
                var user = GetUser(userId);
                return CheckIf(user, permission);
            }
            catch
            {
                return false;
            }
        }

        public bool CheckIf(BaseUser user, string permission)
        {
            if (user == null)
                return false;
                
            return HasPermission(user, permission);
        }

        // Private helper methods
        private BaseUser GetUser(int userId)
        {
            var user = userRepository.Get(userId);
            if (user == null)
                throw new InvalidOperationException($"User with ID {userId} not found.");
            return user;
        }

        private List<string> GetUserPermissions(BaseUser user)
        {
            return user != null ? userTypePermissions[user.Type] : userTypePermissions[UserType.None];
        }

        private bool HasPermission(BaseUser user, string permission)
        {
            var userPermissions = GetUserPermissions(user);
            return userPermissions.Contains(Permission.All) || userPermissions.Contains(permission);
        }

        private void CheckUserAccess(BaseUser requestUser, BaseUser targetUser, string selfPermission, string processPermission)
        {
            var userPermissions = GetUserPermissions(requestUser);

            // Check for "All" permission
            if (userPermissions.Contains(Permission.All))
                return;

            // If targeting self, check self permission
            if (requestUser.Id == targetUser.Id)
            {
                if (userPermissions.Contains(selfPermission))
                    return;
            }

            // Check process permission for targeting others
            if (userPermissions.Contains(processPermission))
                return;

            throw new System.UnauthorizedAccessException("Access denied.");
        }
    }
}
