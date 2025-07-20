using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Users.UpdateUser
{
    public class UpdateUserCommandValidator : IValidator<UpdateUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(UpdateUserCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate UserId
            if (request.UserId <= 0)
            {
                errors.Add("UserId must be a positive integer.");
            }

            // Validate Name (can be null for optional update, but if provided should be valid)
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                if (request.Name.Length > 100)
                {
                    errors.Add("Name must not exceed 100 characters.");
                }
            }

            // Validate firstName (optional, but if provided, must be at least 2 characters)
            if (!string.IsNullOrWhiteSpace(request.firstName))
            {
                if (request.firstName.Length < 2)
                {
                    errors.Add("First name must be at least 2 characters.");
                }
                if (request.firstName.Length > 50)
                {
                    errors.Add("First name must not exceed 50 characters.");
                }
            }

            // Validate lastName (optional, but if provided, must be at least 2 characters)
            if (!string.IsNullOrWhiteSpace(request.lastName))
            {
                if (request.lastName.Length < 2)
                {
                    errors.Add("Last name must be at least 2 characters.");
                }
                if (request.lastName.Length > 50)
                {
                    errors.Add("Last name must not exceed 50 characters.");
                }
            }

            // Validate address (optional, but if provided, must not be empty or whitespace)
            if (request.address != null)
            {
                if (string.IsNullOrWhiteSpace(request.address))
                {
                    errors.Add("Address cannot be empty or whitespace.");
                }
                else if (request.address.Length > 200)
                {
                    errors.Add("Address must not exceed 200 characters.");
                }
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}