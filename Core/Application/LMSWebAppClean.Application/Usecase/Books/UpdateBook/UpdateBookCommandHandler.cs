using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateBookCommandHandler(IPermissionChecker permissionChecker, IRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker;
            this.bookRepository = bookRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new ArgumentException("Title is required.");
                }

                var book = bookRepository.Get(request.BookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {request.BookId} was not found.");
                }

                // Update book properties
                book.Title = request.Title;
                book.Author = request.Author;
                book.PublicationYear = request.Year;
                book.Category = request.Category;
                book.ISBN = request.ISBN;
                book.Synopsis = request.Synopsis;

                var updatedBook = bookRepository.Update(book);
                await unitOfWork.SaveChangesAsync();

                return updatedBook;
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating book with ID {request.BookId}.", ex);
            }
        }
    }
}