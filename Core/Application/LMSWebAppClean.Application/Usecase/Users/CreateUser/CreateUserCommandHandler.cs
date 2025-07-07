using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using MediatR;
using System.Security.Cryptography;

namespace LMSWebAppClean.Application.Usecase.Users.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, BaseUser>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to create users
                permissionChecker.Check(request.AuthId, Permission.UserAdd, "User does not have permission to create users.");

                // Create new user (implementation depends on how BaseUser is instantiated)
                BaseUser user = request.Type switch
                {
                    UserType.Member => new Member { Name = request.Name, Type = UserType.Member },
                    UserType.StaffMinor => new Staff { Name = request.Name, Type = UserType.StaffMinor },
                    UserType.StaffManagement => new Staff { Name = request.Name, Type = UserType.StaffManagement },
                    UserType.None => throw new ArgumentException("Invalid user type"),
                    _ => throw new ArgumentException("Invalid user type"),
                };

                var createdUser = userRepository.Add(user);
                await unitOfWork.SaveChangesAsync();

                return createdUser;
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }
    }
}
