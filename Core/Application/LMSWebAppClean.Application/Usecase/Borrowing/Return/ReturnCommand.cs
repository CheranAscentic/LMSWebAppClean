using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Usecase.Borrowing.Return
{
    public class ReturnCommand : IRequest<Book>, ICommand
    {
        public int AuthId { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }

        public ReturnCommand(int authId, int bookId, int memberId)
        {
            AuthId = authId;
            BookId = bookId;
            MemberId = memberId;
        }
    }
}
