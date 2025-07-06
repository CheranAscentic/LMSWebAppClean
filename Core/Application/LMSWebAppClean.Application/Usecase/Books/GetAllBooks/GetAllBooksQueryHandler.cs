using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;

        public GetAllBooksQueryHandler(IPermissionChecker permissionChecker, IRepository<Book> bookRepository)
        {
            this.permissionChecker = permissionChecker;
            this.bookRepository = bookRepository;
        }

        public async Task<List<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                permissionChecker.Check(request.AuthId, Permission.BookViewAll, "User does not have permission to view all books.");
                var books = bookRepository.GetAll();
                return await Task.FromResult(books);
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving books.", ex);
            }
        }
    }
}