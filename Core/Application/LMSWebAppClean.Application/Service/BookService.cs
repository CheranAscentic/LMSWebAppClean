using LMSWebAppClean.Domain.Model;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<Book> bookRepository;

        public BookService(IUnitOfWork unitOfWork, IPermissionChecker permissionChecker, IRepository<Book> bookRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public Book AddBook(int authId, string title, string? author, int? year, string category)
        {
            permissionChecker.Check(authId, Permission.BookAdd, "User does not have permission to add book.");
            Book newBook = new Book(title, author, year, category);
            var result = bookRepository.Add(newBook);
            unitOfWork.SaveChanges();
            return result;
        }

        public Book GetBook(int authId, int bookId)
        {
            permissionChecker.Check(authId, Permission.BookView, "User does not have permission to view book.");
            var book = bookRepository.Get(bookId) ?? throw new Exception("Book with Title cannot be found");
            return book;
        }

        public List<Book> GetBooks(int authId)
        {
            permissionChecker.Check(authId, Permission.BookViewAll, "User does not have permission to view all books.");
            return bookRepository.GetAll();
        }

        public Book UpdateBook(int authId, int bookId, string? title, string? author, int? year, string? category)
        {
            permissionChecker.Check(authId, Permission.BookUpdate, "User does not have permission to update book.");
            var book = bookRepository.Get(bookId) ?? throw new Exception("Book with Title cannot be found");

            book.Title = title ?? book.Title;
            book.Author = author ?? book.Author;
            book.PublicationYear = year ?? book.PublicationYear;
            book.Category = category ?? book.Category;

            var result = bookRepository.Update(book);
            unitOfWork.SaveChanges();
            return result;
        }

        public Book RemoveBook(int authId, int bookId)
        {
            permissionChecker.Check(authId, Permission.BookDelete, "User does not have permission to delete book.");
            var result = bookRepository.Remove(bookId) ?? throw new Exception("Book with Title cannot be found");
            unitOfWork.SaveChanges();
            return result;
        }
    }
}
