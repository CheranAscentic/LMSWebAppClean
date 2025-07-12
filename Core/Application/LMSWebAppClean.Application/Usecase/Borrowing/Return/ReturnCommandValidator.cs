using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Borrowing.Return
{
    public class ReturnCommandValidator : IValidator<ReturnCommand>
    {
        public Task<ValidationResult> ValidateAsync(ReturnCommand request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

            // Validate BookId
            if (request.BookId <= 0)
            {
                errors.Add("BookId must be a positive integer.");
            }

            // Validate MemberId
            if (request.MemberId <= 0)
            {
                errors.Add("MemberId must be a positive integer.");
            }

            var result = errors.Any() 
                ? ValidationResult.Failure(errors) 
                : ValidationResult.Success();

            return Task.FromResult(result);
        }
    }
}