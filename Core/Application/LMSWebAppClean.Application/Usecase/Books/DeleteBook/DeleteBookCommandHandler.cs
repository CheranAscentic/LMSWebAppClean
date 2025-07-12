using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Book>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteBookCommandHandler(IPermissionChecker permissionChecker, IRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker;
            this.bookRepository = bookRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Book> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to delete books
                //permissionChecker.Check(request.AuthId, Permission.BookDelete, "User does not have permission to delete books.");

                var deletedBook = bookRepository.Remove(request.BookId);
                
                if (deletedBook == null)
                {
                    throw new KeyNotFoundException($"Book with ID {request.BookId} was not found.");
                }

                await unitOfWork.SaveChangesAsync();

                return deletedBook;
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting book with ID {request.BookId}.", ex);
            }
        }
    }
}