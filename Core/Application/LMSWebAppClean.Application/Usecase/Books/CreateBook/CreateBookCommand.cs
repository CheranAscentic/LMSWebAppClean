using LMSWebAppClean.Application.Interface;
using LMSWebAppClean.Domain.Model;
using MediatR;

namespace LMSWebAppClean.Application.Usecase.Books.CreateBook
{
    public class CreateBookCommand : IRequest<Book>, ICommand
    {
        public int AuthId { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string Category { get; set; }

        public CreateBookCommand(int authId, string title, string? author, int? year, string category)
        {
            AuthId = authId;
            Title = title;
            Author = author;
            Year = year;
            Category = category;
        }
    }
}