using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.GetBookById
{
    public class GetBookByIdQuery : IRequest<Book>, IQuery
    {
        public int BookId { get; set; }

        public GetBookByIdQuery(int bookId)
        {
            BookId = bookId;
        }
    }
}