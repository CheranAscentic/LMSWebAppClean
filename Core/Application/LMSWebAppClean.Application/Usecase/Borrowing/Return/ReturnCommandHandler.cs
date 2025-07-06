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

namespace LMSWebAppClean.Application.Usecase.Borrowing.Return
{
    public class ReturnCommandHandler : IRequestHandler<ReturnCommand, Book>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPermissionChecker permissionChecker;
        private readonly IRepository<BaseUser> userRepository;
        private readonly IRepository<Book> bookRepository;

        public ReturnCommandHandler(
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

        public async Task<Book> Handle(ReturnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                permissionChecker.Check(request.AuthId, Permission.BorrowReturn, "You do not have permission to return books");

                var book = bookRepository.Get(request.BookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book with ID {request.BookId} not found");
                }

                var user = userRepository.Get(request.MemberId);
                if (user == null || !(user is Member member))
                {
                    throw new InvalidOperationException("Only members can return books");
                }

                if (!member.BorrowedBooks.Any(b => b.Id == request.BookId))
                {
                    throw new InvalidOperationException($"Member with ID {request.MemberId} has not borrowed book with ID {request.BookId}");
                }

                book.Available = true;
                bookRepository.Update(book);

                member.BorrowedBooks.RemoveAll(b => b.Id == request.BookId);
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
                throw new Exception($"An error occurred while returning book with ID {request.BookId}.", ex);
            }
        }
    }
}
