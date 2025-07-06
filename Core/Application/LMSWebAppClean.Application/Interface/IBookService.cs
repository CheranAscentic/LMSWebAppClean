using LMSWebAppClean.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.Interface
{
    public interface IBookService
    {
        public Book AddBook(int authId, string title, string? author, int? year, string category);
        public Book GetBook(int authId, int bookId);
        public List<Book> GetBooks(int authId);
        public Book UpdateBook(int authId, int bookId, string title, string? author, int? year, string category);
        public Book RemoveBook(int authId, int bookId);

    }
}
