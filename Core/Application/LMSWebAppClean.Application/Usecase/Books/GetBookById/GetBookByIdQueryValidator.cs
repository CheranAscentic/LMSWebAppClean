using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Books.GetBookById
{
    public class GetBookByIdQueryValidator : IValidator<GetBookByIdQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetBookByIdQuery request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate BookId
            if (request.BookId <= 0)
            {
                errors.Add("BookId must be a positive integer.");
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}