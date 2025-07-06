using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.DeleteBook
{
    public class DeleteBookCommand : IRequest<Book>, ICommand
    {
        public int AuthId { get; set; }
        public int BookId { get; set; }

        public DeleteBookCommand(int authId, int bookId)
        {
            AuthId = authId;
            BookId = bookId;
        }
    }
}