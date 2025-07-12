using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseUser>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserCommandHandler(IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<BaseUser> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to delete users
                //permissionChecker.Check(request.AuthId, Permission.UserDelete, "User does not have permission to delete users.");

                // Remove user
                var user = userRepository.Remove(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
                }

                await unitOfWork.SaveChangesAsync();

                return user;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting user with ID {request.UserId}.", ex);
            }
        }
    }
}
