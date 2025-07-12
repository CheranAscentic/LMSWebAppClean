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

        public CreateBookCommand(string title, string? author, int? year, string category)
        {
            Title = title;
            Author = author;
            Year = year;
            Category = category;
        }
    }
}