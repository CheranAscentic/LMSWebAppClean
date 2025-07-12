using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.Application.Usecase.Identity.RegisterUser
{
    public class RegisterUserCommandValidator : IValidator<RegisterUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(RegisterUserCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate Name
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                errors.Add("Name is required.");
            }
            else if (request.Name.Length < 2)
            {
                errors.Add("Name must be at least 2 characters long.");
            }
            else if (request.Name.Length > 100)
            {
                errors.Add("Name must not exceed 100 characters.");
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(request.Email))
            {
                errors.Add("Email format is invalid.");
            }
            else if (request.Email.Length > 254)
            {
                errors.Add("Email must not exceed 254 characters.");
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add("Password is required.");
            }
            else
            {
                var passwordErrors = ValidatePassword(request.Password);
                errors.AddRange(passwordErrors);
            }

            // Validate UserType
            if (string.IsNullOrWhiteSpace(request.UserType) || (request.UserType == UserType.None))
            {
                errors.Add("Invalid user type.");
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

        private static List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (password.Length < 12)
            {
                errors.Add("Password must be at least 12 characters long.");
            }

            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }

            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one digit.");
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errors.Add("Password must contain at least one special character.");
            }

            return errors;
        }
    }
}