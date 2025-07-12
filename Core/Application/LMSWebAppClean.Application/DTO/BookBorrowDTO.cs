using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO
{
    public class BookBorrowDTO : ICommand
    {
        public int MemberId { get; set; }
        public int BookId { get; set; }
    }
}
