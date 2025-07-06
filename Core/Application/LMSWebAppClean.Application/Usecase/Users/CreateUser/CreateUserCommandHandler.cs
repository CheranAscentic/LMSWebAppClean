using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using MediatR;

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

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    throw new ArgumentException("Username is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    throw new ArgumentException("Email is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new ArgumentException("Password is required.");
                }

                // Determine user type based on role
                UserType userType = DetermineUserType(request.Role);

                // Create new user (implementation depends on how BaseUser is instantiated)
                var user = CreateUserInstance(request.Username, userType);
                user.Name = request.Username;
                
                // Add other properties as needed
                // Note: This might require a specific user factory or implementation details

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

        private UserType DetermineUserType(string? role)
        {
            if (string.IsNullOrEmpty(role))
                return UserType.Member;

            return role.ToLower() switch
            {
                "staff" => UserType.StaffMinor,
                "manager" => UserType.StaffManagement,
                "admin" => UserType.StaffManagement,
                _ => UserType.Member
            };
        }

        private BaseUser CreateUserInstance(string name, UserType userType)
        {
            // This is a simplified implementation - you'll need to replace with proper user creation
            // based on your actual BaseUser implementation
            return UserFactory.CreateUser(name, userType);
        }
    }

    // Example factory - replace with your actual implementation
    internal static class UserFactory
    {
        public static BaseUser CreateUser(string name, UserType userType)
        {
            // You'll need to implement this based on your actual user class hierarchy
            throw new NotImplementedException("User creation needs to be implemented based on the actual user class hierarchy.");
        }
    }
}
