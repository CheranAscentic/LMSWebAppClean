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
        private readonly IRepository<BaseUser> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateUserCommandHandler(IRepository<BaseUser> userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create new user (implementation depends on how BaseUser is instantiated)
                BaseUser user = request.Type switch
                {
                    UserType.Member => new Member { Name = request.Name, Type = UserType.Member },
                    UserType.StaffMinor => new Staff { Name = request.Name, Type = UserType.StaffMinor },
                    UserType.StaffManagement => new Staff { Name = request.Name, Type = UserType.StaffManagement },
                    UserType.None => throw new ArgumentException("Invalid user type"),
                    _ => throw new ArgumentException("Invalid user type"),
                };

                user.Email = request.Email;

                // Set the ID if provided, otherwise let the database auto-generate it
                if (request.Id.HasValue)
                {
                    user.Id = request.Id.Value;
                }

                var createdUser = userRepository.Add(user);
                await unitOfWork.SaveChangesAsync();

                return createdUser;
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
