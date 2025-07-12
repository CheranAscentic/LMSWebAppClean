using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Application.DTO.Auth;
using MediatR;
using LMSWebAppClean.Application.DTO;

namespace LMSWebAppClean.Application.Usecase.Identity.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginDTO>
    {
        private readonly IUserLoginService userLoginService;

        public LoginUserCommandHandler(IUserLoginService userLoginService)
        {
            this.userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
        }

        public async Task<LoginDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await userLoginService.LoginAsync(
                    request.Email, 
                    request.Password, 
                    request.RememberMe);

                if (!result.IsSuccess)
                {
                    throw new UnauthorizedAccessException(result.ErrorMessage ?? "Login failed.");
                }

                return result.loginDTO!;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the login request, " + ex.Message, ex);
            }
        }
    }
}