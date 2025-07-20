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
                var user = userRepository.Get(request.UserId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {request.UserId} was not found.");
                }

                // Update user properties if provided
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    user.Name = request.Name;
                }

                if (!string.IsNullOrWhiteSpace(request.firstName))
                {
                    user.FirstName = request.firstName;
                }

                if (!string.IsNullOrWhiteSpace(request.lastName))
                {
                    user.LastName = request.lastName;
                }

                // For address, allow null assignment to clear value if needed
                if (request.address != null)
                {
                    user.Address = request.address;
                }

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
    }
}
