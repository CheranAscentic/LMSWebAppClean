using LMSWebAppClean.Application.Interface;

namespace LMSWebAppClean.Application.DTO
{
    public class UserUpdateDTO : ICommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}