using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks
{
    public class GetBorrowedBooksQueryHandler : IRequestHandler<GetBorrowedBooksQuery, List<Book>>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;

        public GetBorrowedBooksQueryHandler(
            IPermissionChecker permissionChecker,
            IRepository<BaseUser> userRepository)
        {
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<Book>> Handle(GetBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                permissionChecker.Check(request.AuthId, Permission.BorrowViewBorrowedBooks, "You do not have permission to view borrowed books");
                
                var user = userRepository.Get(request.MemberId);
                if (user == null || !(user is Member member))
                {
                    throw new KeyNotFoundException($"Member with ID {request.MemberId} not found");
                }

                return await Task.FromResult(member.BorrowedBooks);
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
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
