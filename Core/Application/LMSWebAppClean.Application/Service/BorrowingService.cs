using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Service
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IRepository<Book> bookRepository;

        public BorrowingService(IUnitOfWork unitOfWork, IPermissionChecker permissionChecker, IRepository<BaseUser> userRepository, IRepository<Book> bookRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public Book BorrowBook(int authId, int bookId, int memberId)
        {
            permissionChecker.Check(authId, Permission.BorrowBook, "You do not have permission to borrow books");

            var book = bookRepository.Get(bookId) ?? throw new Exception("Book not found");

            if (!book.Available)
                throw new Exception("Book is not available for borrowing");

            var user = userRepository.Get(memberId);

            if (user == null || !(user is Member member))
                throw new Exception("Only members can borrow books");

            book.Available = false;
            bookRepository.Update(book);

            member.BorrowedBooks.Add(book);
            userRepository.Update(member);

            unitOfWork.SaveChanges();

            return book;
        }

        public List<Book> GetBorrowedBooks(int authId, int memberId)
        {
            permissionChecker.Check(authId, Permission.BorrowViewBorrowedBooks, "You do not have permission to view borrowed books");
            var user = userRepository.Get(memberId);

            if (user == null || !(user is Member member))
                throw new Exception("Member not found");

            return member.BorrowedBooks;
        }

        public Book ReturnBook(int authId, int bookId, int memberId)
        {
            permissionChecker.Check(authId, Permission.BorrowReturn, "You do not have permission to return books");

            var book = bookRepository.Get(bookId) ?? throw new Exception("Book not found");

            var user = userRepository.Get(memberId);

            if (user == null || !(user is Member member))
                throw new Exception("Only members can return books");

            if (!member.BorrowedBooks.Any(b => b.Id == bookId))
                throw new Exception("Member has not borrowed this book");

            book.Available = true;
            bookRepository.Update(book);

            member.BorrowedBooks.RemoveAll(b => b.Id == bookId);
            userRepository.Update(member);

            unitOfWork.SaveChanges();

            return book;
        }
    }
}
