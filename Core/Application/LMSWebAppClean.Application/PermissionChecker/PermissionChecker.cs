using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.PermissionChecker
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly Dictionary<UserType, List<Permission>> userTypePermissions;
        IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public PermissionChecker(IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            userTypePermissions = new Dictionary<UserType, List<Permission>>
            {
                {
                    UserType.None,
                    new List<Permission>
                    {
                        Permission.BookView,
                        Permission.BookViewAll
                    }
                },
                {
                    UserType.Member,
                    new List<Permission>
                    {
                        Permission.BookView,
                        Permission.BookViewAll,
                        Permission.UserView,
                        Permission.UserUpdate,
                        Permission.BorrowBook,
                        Permission.BorrowReturn,
                        Permission.BorrowViewBorrowedBooks
                    }
                },
                {
                    UserType.StaffMinor,
                    new List<Permission>
                    {
                        Permission.BookView,
                        Permission.BookViewAll,
                        Permission.BookAdd,
                        Permission.BookUpdate,
                        Permission.BookDelete,
                        Permission.UserAdd,
                        Permission.BorrowViewBorrowedBooks
                    }
                },
                {
                    UserType.StaffManagement,
                    new List<Permission>
                    {
                        Permission.BookView,
                        Permission.BookViewAll,
                        Permission.BookAdd,
                        Permission.BookUpdate,
                        Permission.BookDelete,
                        Permission.UserView,
                        Permission.UserViewAll,
                        Permission.UserAdd,
                        Permission.UserUpdate,
                        Permission.UserDelete,
                        Permission.BorrowViewBorrowedBooks
                    }
                }
            };
        }

        public void Check(int userId, Permission permission, string errMsg)
        {
            List<Permission> perms;
            try
            {
                //var user = userRepository.Get(userId);
                var user = userRepository.Get(userId);
                perms = userTypePermissions[user.Type];
            }
            catch (Exception ex) 
            {
                perms = userTypePermissions[UserType.None];
            }


            if (perms.Contains(Permission.None))
            {
                throw new UnauthorizedAccessException(errMsg);
            }
            if (perms.Contains(Permission.All))
            {
                return;
            }
            if (perms.Contains(permission))
            {
                return;
            }
            throw new UnauthorizedAccessException(errMsg);
        }
    }
}
