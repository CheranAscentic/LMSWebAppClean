using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Books.GetAllBooks
{
    public class GetAllBooksQueryValidator : IValidator<GetAllBooksQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetAllBooksQuery request, CancellationToken cancellationToken = default)
        {
            // GetAllBooksQuery has no parameters to validate, so it's always valid
            var result = ValidationResult.Success();
            return Task.FromResult(result);
        }
    }
}