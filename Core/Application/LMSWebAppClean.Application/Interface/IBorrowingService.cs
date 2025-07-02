using LMSWebAppClean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Interface
{
    public interface IBorrowingService
    {
        Book BorrowBook(int authId, int bookId, int memberId);
        Book ReturnBook(int authId, int bookId, int memberId);
        List<Book> GetBorrowedBooks(int authId, int memberId);
    }
}
