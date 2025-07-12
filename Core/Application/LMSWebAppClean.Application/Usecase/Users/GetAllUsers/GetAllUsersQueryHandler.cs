using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<BaseUser>>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;

        public GetAllUsersQueryHandler(IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository)
        {
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<BaseUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to view all users
                //permissionChecker.Check(request.AuthId, Permission.UserViewAll, "User does not have permission to view all users.");

                // Get all users
                return userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
        }
    }
}
