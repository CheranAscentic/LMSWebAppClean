using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<List<Book>>, IQuery
    {
        public GetAllBooksQuery() {}
    }
}
