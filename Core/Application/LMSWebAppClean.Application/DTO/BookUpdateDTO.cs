namespace LMSWebAppClean.Application.DTO
{
    public class BookUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string? Category { get; set; }
    }
}