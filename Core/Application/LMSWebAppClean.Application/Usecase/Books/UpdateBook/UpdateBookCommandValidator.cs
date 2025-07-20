using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Books.UpdateBook
{
    public class UpdateBookCommandValidator : IValidator<UpdateBookCommand>
    {
        public Task<ValidationResult> ValidateAsync(UpdateBookCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate BookId
            if (request.BookId <= 0)
            {
                errors.Add("BookId must be a positive integer.");
            }

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
            if (!string.IsNullOrWhiteSpace(request.Category) && request.Category.Length > 50)
            {
                errors.Add("Category must not exceed 50 characters.");
            }

            if (!string.IsNullOrWhiteSpace(request.ISBN))
            {
                if (request.ISBN.Length < 10)
                {
                    errors.Add("ISBN must be at least 10 characters.");
                }
                else if (request.ISBN.Length > 20)
                {
                    errors.Add("ISBN must not exceed 20 characters.");
                }
            }

            // Validate Synopsis (optional, but if provided, must be at least 10 and at most 1000 characters)
            if (!string.IsNullOrWhiteSpace(request.Synopsis))
            {
                if (request.Synopsis.Length < 10)
                {
                    errors.Add("Synopsis must be at least 10 characters.");
                }
                else if (request.Synopsis.Length > 1000)
                {
                    errors.Add("Synopsis must not exceed 1000 characters.");
                }
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}