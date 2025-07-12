using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Identity.LoginUser
{
    public class LoginUserCommandValidator : IValidator<LoginUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(LoginUserCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate Email
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(request.Email))
            {
                errors.Add("Email format is invalid.");
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add("Password is required.");
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}