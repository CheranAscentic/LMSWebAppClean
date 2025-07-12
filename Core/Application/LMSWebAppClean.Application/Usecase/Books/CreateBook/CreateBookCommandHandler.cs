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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the book.", ex);
            }
        }
    }
}