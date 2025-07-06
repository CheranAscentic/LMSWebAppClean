using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Users.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, BaseUser>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserHandler(IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseUser> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to update users
                permissionChecker.Check(request.AuthId, Permission.UserUpdate, "User does not have permission to update users.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    throw new ArgumentException("Username is required.");
                }

                /*if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new ArgumentException("Email is required.");
                }*/

                // Get existing user
                var user = userRepository.Get(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
                }

                // Update user properties
                user.Name = request.Username;

                // If the user's role should be updated, update the user type
                if (!string.IsNullOrEmpty(request.Role))
                {
                    UserType userType = DetermineUserType(request.Role);
                    user.Type = userType;
                }

                // Additional properties would be updated here
                // Note: Email update might require additional handling

                var updatedUser = userRepository.Update(user);
                await unitOfWork.SaveChangesAsync();

                return updatedUser;
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating user with ID {request.UserId}.", ex);
            }
        }

        private UserType DetermineUserType(string role)
        {
            return role.ToLower() switch
            {
                "staff" => UserType.StaffMinor,
                "manager" => UserType.StaffManagement,
                "admin" => UserType.StaffManagement,
                _ => UserType.Member
            };
        }
    }
}
