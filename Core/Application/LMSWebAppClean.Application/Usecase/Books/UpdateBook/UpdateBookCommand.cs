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

        public UpdateBookCommand(int bookId, string title, string? author, int? year, string category)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            Year = year;
            Category = category;
        }
    }
}