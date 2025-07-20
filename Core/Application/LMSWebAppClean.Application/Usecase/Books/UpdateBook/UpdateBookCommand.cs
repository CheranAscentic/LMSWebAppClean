using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.UpdateBook
{
    public class UpdateBookCommand : IRequest<Book>, ICommand
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string? Category { get; set; }
        public string? ISBN { get; set; }
        public string? Synopsis { get; set; }

        public UpdateBookCommand(int bookId, string title, string? author, int? year, string category, string? isbn, string? synopsis)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            Year = year;
            Category = category;
            ISBN = isbn;
            Synopsis = synopsis;
        }
    }
}