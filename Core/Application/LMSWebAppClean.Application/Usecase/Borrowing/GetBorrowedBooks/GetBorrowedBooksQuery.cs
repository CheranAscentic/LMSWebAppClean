using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Borrowing.GetBorrowedBooks
{
    public class GetBorrowedBooksQuery : IRequest<List<Book>>, IQuery
    {
        public int MemberId { get; set; }

        public GetBorrowedBooksQuery(int memberId)
        {
            MemberId = memberId;
        }
    }
}
