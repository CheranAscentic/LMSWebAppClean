using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Books.CreateBook
{
    public class CreateBookCommandValidator : IValidator<CreateBookCommand>
    {
        public Task<ValidationResult> ValidateAsync(CreateBookCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate Title
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                errors.Add("Title is required.");
            }
            else if (request.Title.Length > 200)
            {
                errors.Add("Title must not exceed 200 characters.");
            }

            // Validate Author
            if (!string.IsNullOrWhiteSpace(request.Author) && request.Author.Length > 100)
            {
                errors.Add("Author name must not exceed 100 characters.");
            }

            // Validate Year
            if (request.Year.HasValue)
            {
                var currentYear = DateTime.Now.Year;
                if (request.Year.Value < 1000 || request.Year.Value > currentYear + 1)
                {
                    errors.Add($"Publication year must be between 1000 and {currentYear + 1}.");
                }
            }

            // Validate Category
            if (string.IsNullOrWhiteSpace(request.Category))
            {
                errors.Add("Category is required.");
            }
            else if (request.Category.Length > 50)
            {
                errors.Add("Category must not exceed 50 characters.");
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}