using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;

        public GetBookByIdQueryHandler(IPermissionChecker permissionChecker, IRepository<Book> bookRepository)
        {
            this.permissionChecker = permissionChecker;
            this.bookRepository = bookRepository;
        }

        public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //permissionChecker.Check(request.AuthId, Permission.BookView, "User does not have permission to view book details.");
                var book = bookRepository.Get(request.BookId);
                
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {request.BookId} was not found.");
                }
                
                return await Task.FromResult(book);
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving book with ID {request.BookId}.", ex);
            }
        }
    }
}