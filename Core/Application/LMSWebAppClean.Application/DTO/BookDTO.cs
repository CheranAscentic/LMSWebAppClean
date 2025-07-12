using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO
{
    public class BookDTO : ICommand
    {
        public string Title { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string Category { get; set; }
    }
}
