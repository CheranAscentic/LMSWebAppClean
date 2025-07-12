using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, BaseUser>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;

        public GetUserByIdQueryHandler(IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository)
        {
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<BaseUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to view user
                //permissionChecker.Check(request.AuthId, Permission.UserView, "User does not have permission to view user details.");

                // Get user by id
                var user = userRepository.Get(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
                }

                return user;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving user with ID {request.UserId}.", ex);
            }
        }
    }
}
