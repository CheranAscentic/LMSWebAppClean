using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Borrowing.Borrow
{
    public class BorrowCommandHandler : IRequestHandler<BorrowCommand, Book>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IRepository<Book> bookRepository;

        public BorrowCommandHandler(
            IUnitOfWork unitOfWork, 
            IPermissionChecker permissionChecker, 
            IRepository<BaseUser> userRepository, 
            IRepository<Book> bookRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.permissionChecker = permissionChecker ?? throw new ArgumentNullException(nameof(permissionChecker));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<Book> Handle(BorrowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                permissionChecker.Check(request.AuthId, Permission.BorrowBook, "You do not have permission to borrow books");

                var book = bookRepository.Get(request.BookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {request.BookId} not found");
                }

                if (!book.Available)
                {
                    throw new InvalidOperationException($"Book with ID {request.BookId} is not available for borrowing");
                }

                var user = userRepository.Get(request.MemberId);
                if (user == null || !(user is Member member))
                {
                    throw new InvalidOperationException("Only members can borrow books");
                }

                book.Available = false;
                bookRepository.Update(book);

                member.BorrowedBooks.Add(book);
                userRepository.Update(member);

                await unitOfWork.SaveChangesAsync();

                return book;
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Re-throw permission exceptions
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw not found exceptions
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw business rule violations
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while borrowing book with ID {request.BookId}.", ex);
            }
        }
    }
}
