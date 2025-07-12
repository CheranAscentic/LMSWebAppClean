using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Enum;

namespace LMSWebAppClean.Application.Usecase.Users.CreateUser
{
    public class CreateUserCommandValidator : IValidator<CreateUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate Name
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                errors.Add("Name is required.");
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

            // Validate UserType
            var validUserTypes = new[] { UserType.Member, UserType.StaffMinor, UserType.StaffManagement };
            if (string.IsNullOrWhiteSpace(request.Type) || !validUserTypes.Contains(request.Type))
            {
                errors.Add("Invalid user type. Valid types are: Member, StaffMinor, StaffManagement.");
            }

            // Validate ID if provided
            if (request.Id.HasValue && request.Id.Value <= 0)
            {
                errors.Add("ID must be a positive integer when provided.");
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