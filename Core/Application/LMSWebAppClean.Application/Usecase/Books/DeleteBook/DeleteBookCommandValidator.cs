using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Books.DeleteBook
{
    public class DeleteBookCommandValidator : IValidator<DeleteBookCommand>
    {
        public Task<ValidationResult> ValidateAsync(DeleteBookCommand request, CancellationToken cancellationToken = default)
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