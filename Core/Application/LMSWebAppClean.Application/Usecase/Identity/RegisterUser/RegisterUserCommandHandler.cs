using LMSWebAppClean.Application.Interface;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Identity.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRegistrationService userRegistrationService;
        private readonly IMediator mediator;

        public RegisterUserCommandHandler(IUserRegistrationService userRegistrationService, IMediator mediator)
        {
            this.userRegistrationService = userRegistrationService ?? throw new ArgumentNullException(nameof(userRegistrationService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Delegate to the registration service
                var result = await userRegistrationService.RegisterUserAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.UserType,
                    cancellationToken);

                if (!result.IsSuccess)
                {
                    var errorMessage = string.Join(", ", result.Errors);
                    throw new InvalidOperationException(errorMessage);
                }

                return result.IdentityUserId!;
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw business logic exceptions
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the registration request.", ex);
            }
        }
    }
}