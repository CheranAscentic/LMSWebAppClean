using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Users.DeleteUser
{
    public class DeleteUserCommandValidator : IValidator<DeleteUserCommand>
    {
        public Task<ValidationResult> ValidateAsync(DeleteUserCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate UserId
            if (request.UserId <= 0)
            {
                errors.Add("UserId must be a positive integer.");
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}