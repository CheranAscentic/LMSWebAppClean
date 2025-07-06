using LMSWebAppClean.Application.Interface;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<List<Domain.Model.Book>>, IQuery
    {
        public int AuthId { get; set; }

        public GetAllBooksQuery(int authId)
        {
            AuthId = authId;
        }
    }
}
