using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Users.GetAllUsers
{
    public class GetAllUsersQueryValidator : IValidator<GetAllUsersQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetAllUsersQuery request, CancellationToken cancellationToken = default)
        {
            // GetAllUsersQuery has no parameters to validate, so it's always valid
            var result = ValidationResult.Success();
            return Task.FromResult(result);
        }
    }
}