using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks
{
    public class GetBorrowedBooksQueryHandler : IRequestHandler<GetBorrowedBooksQuery, List<Book>>
    {
        private readonly IRepository<BaseUser> userRepository;

        public GetBorrowedBooksQueryHandler(
            IRepository<BaseUser> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<Book>> Handle(GetBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Load member with their borrowed books to ensure the collection is populated
                var user = userRepository.GetWithIncludes(request.MemberId, "BorrowedBooks");
                if (user == null || !(user is Member member))
                {
                    throw new KeyNotFoundException($"Member with ID {request.MemberId} not found");
                }

                return await Task.FromResult(member.BorrowedBooks);
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving borrowed books for member with ID {request.MemberId}.", ex);
            }
        }
    }
}
