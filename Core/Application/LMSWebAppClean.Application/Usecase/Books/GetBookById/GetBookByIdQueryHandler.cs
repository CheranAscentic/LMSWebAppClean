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

        public Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            permissionChecker.Check(request.AuthId, Permission.BookView, "User does not have permission to view book.");
            var book = bookRepository.Get(request.BookId) ?? throw new Exception("Book with Id cannot be found");
            return Task.FromResult(book);
        }
    }
}