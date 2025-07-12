using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks
{
    public class GetBorrowedBooksQueryValidator : IValidator<GetBorrowedBooksQuery>
    {
        public Task<ValidationResult> ValidateAsync(GetBorrowedBooksQuery request, CancellationToken cancellationToken = default)
        {
            var errors = new List<string>();

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