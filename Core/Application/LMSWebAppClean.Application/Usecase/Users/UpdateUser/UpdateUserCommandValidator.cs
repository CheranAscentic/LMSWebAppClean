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

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}