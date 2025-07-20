using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.CreateBook
{
    public class CreateBookCommand : IRequest<Book>, ICommand
    {
        public string Title { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string Category { get; set; }
        public string? ISBN { get; set; }
        public string? Synopsis { get; set; }

        public CreateBookCommand(string title, string? author, int? year, string category, string? isbn, string? synopsis)
        {
            Title = title;
            Author = author;
            Year = year;
            Category = category;
            ISBN = isbn;
            Synopsis = synopsis;
        }
    }
}