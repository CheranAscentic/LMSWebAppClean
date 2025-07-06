using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Auth.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseUser>
    {
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public RegisterCommandHandler(IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<BaseUser> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    throw new ArgumentException("User name is required.");
                }

                // Create user based on type
                BaseUser newUser;
                switch (request.Type)
                {
                    case UserType.Member:
                        newUser = new Member(request.Name);
                        break;
                    case UserType.StaffMinor:
                    case UserType.StaffManagement:
                        newUser = new Staff(request.Name, request.Type);
                        break;
                    default:
                        throw new ArgumentException("Invalid user type");
                }
                
                var createdUser = userRepository.Add(newUser);
                await unitOfWork.SaveChangesAsync();

                return createdUser;
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }
    }
}
