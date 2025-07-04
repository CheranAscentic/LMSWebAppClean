using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateBookCommandHandler(IPermissionChecker permissionChecker, IRepository<Book> bookRepository, IUnitOfWork unitOfWork)
        {
            this.permissionChecker = permissionChecker;
            this.bookRepository = bookRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user has permission to create books
                permissionChecker.Check(request.AuthId, Permission.BookAdd, "User does not have permission to create books.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    throw new ArgumentException("Title is required.");
                }

                var book = new Book(
                    request.Title,
                    request.Author,
                    request.Year,
                    request.Category
                );

                var createdBook = bookRepository.Add(book);

                await unitOfWork.SaveChangesAsync();

                return createdBook;
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (ArgumentException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the book.", ex);
            }
        }
    }
}